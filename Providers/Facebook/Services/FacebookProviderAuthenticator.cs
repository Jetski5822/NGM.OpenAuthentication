using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Facebook;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OAuth;
using NGM.OpenAuthentication.Core.Results;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Providers.Facebook.Services {
    [OrchardFeature("Authentication.Facebook")]
    public class FacebookProviderAuthenticator : IOAuthProviderFacebookAuthenticator {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthenticator _authenticator;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;

        private FacebookApplication _facebookApplication;

        public FacebookProviderAuthenticator(IOrchardServices orchardServices,
            IAuthenticator authenticator,
            IOpenAuthenticationService openAuthenticationService,
            IScopeProviderPermissionService scopeProviderPermissionService) {
            _orchardServices = orchardServices;
            _authenticator = authenticator;
            _openAuthenticationService = openAuthenticationService;
            _scopeProviderPermissionService = scopeProviderPermissionService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private FacebookApplication FacebookApplication {
            get { return _facebookApplication ?? (_facebookApplication = new FacebookApplication(ClientKeyIdentifier, ClientSecret)); }
        }


        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.FacebookClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.FacebookClientSecret; }
        }

        public bool IsConsumerConfigured {
            get { return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret); }
        }

        public AuthenticationState Authenticate(string returnUrl) {
            FacebookOAuthResult oAuthResult;
            if (FacebookOAuthResult.TryParse(HttpContext.Current.Request.Url, out oAuthResult)) {
                return TranslateResponseState(returnUrl, oAuthResult);
            }

            return GenerateRequestState(returnUrl);
        }

        private AuthenticationState TranslateResponseState(string returnUrl, FacebookOAuthResult oAuthResult) {
            if (oAuthResult.IsSuccess) {
                var parameters = new OAuthAuthenticationParameters(Provider) {
                    ExternalIdentifier = GetAccessToken(oAuthResult.Code),
                    OAuthToken = oAuthResult.Code,
                    OAuthAccessToken = GetAccessToken(oAuthResult.Code)
                };

                if (_openAuthenticationService.GetSettings().Record.AutoRegisterEnabled)
                    GetUserName(parameters);

                var result = _authenticator.Authenticate(parameters);

                return new AuthenticationState(returnUrl, result.Status);
            }

            _orchardServices.Notifier.Error(T("Reason: {0}, Description: {1}", oAuthResult.ErrorReason, oAuthResult.ErrorDescription));
            return new AuthenticationState(returnUrl, Statuses.ErrorAuthenticating);
        }

        private void GetUserName(OAuthAuthenticationParameters parameters) {
            var client = new FacebookClient(parameters.OAuthAccessToken);
            var me = client.Get("/me");

            var claimsTranslator = new FacebookClaimsTranslator();
            var claims = claimsTranslator.Translate((IDictionary<string, object>)me);

            parameters.AddClaim(claims);
        }

        private AuthenticationState GenerateRequestState(string returnUrl) {
            var facebookClient = new FacebookOAuthClient(FacebookApplication);

            var extendedPermissions = _scopeProviderPermissionService.Get(Provider).Where(o => o.IsEnabled).Select(o => o.Scope).ToArray();
            var parameters = new Dictionary<string, object> {
                {"redirect_uri", GenerateCallbackUri() }
            };

            if (extendedPermissions.Any()) {
                var scope = new StringBuilder();
                scope.Append(string.Join(",", extendedPermissions));
                parameters["scope"] = scope.ToString();
            }

            var result = new RedirectResult(facebookClient.GetLoginUrl(parameters).ToString());

            return new AuthenticationState(returnUrl, Statuses.RequiresRedirect) { Result = result };
        }

        private Uri GenerateCallbackUri() {
            var builder = new UriBuilder(_orchardServices.WorkContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            var path = _orchardServices.WorkContext.HttpContext.Request.ApplicationPath + "/OAuth/LogOn/" + Provider;
            builder.Path = path.Replace(@"//", @"/");

            return builder.Uri;
        }

        public AccessControlProvider Provider {
            get { return new FacebookAccessControlProvider(); }
        }

        public FacebookClient GetClient(IUser user) {
            var parameters = new OAuthAuthenticationParameters(Provider);
            var identifier = _openAuthenticationService.GetExternalIdentifiersFor(user).Where(o => o.HashedProvider == parameters.Provider.Hash)
                .List()
                .First();

            return !string.IsNullOrEmpty(identifier.OAuthAccessToken) ? new FacebookClient(identifier.OAuthAccessToken) : null;
        }

        private string GetAccessToken(string accesscode) {
            var cl = new FacebookOAuthClient(FacebookApplication);
            cl.RedirectUri = GenerateCallbackUri();
            cl.AppId = FacebookApplication.AppId;
            cl.AppSecret = FacebookApplication.AppSecret;
            var dict = (JsonObject)cl.ExchangeCodeForAccessToken(accesscode, new Dictionary<string, object> { { "permissions", "offline_access" } });
            
            return dict.Values.ElementAt(0).ToString();
        }
    }
}