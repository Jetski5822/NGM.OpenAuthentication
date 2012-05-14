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

        private FacebookClient _facebookClient;

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

        private FacebookClient FacebookClient {
            get { return _facebookClient ?? (_facebookClient = new FacebookClient { AppId = ClientKeyIdentifier, AppSecret = ClientSecret }); }
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
            if (FacebookClient.TryParseOAuthCallbackUrl(HttpContext.Current.Request.Url, out oAuthResult)) {
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
            var extendedPermissions = _scopeProviderPermissionService.Get(Provider).Where(o => o.IsEnabled).Select(o => o.Scope).ToArray();
            var parameters = new Dictionary<string, object> {
                {"redirect_uri", GenerateCallbackUri() }
            };

            if (extendedPermissions.Any()) {
                var scope = new StringBuilder();
                scope.Append(string.Join(",", extendedPermissions));
                parameters["scope"] = scope.ToString();
            }

            var result = new RedirectResult(_facebookClient.GetLoginUrl(parameters).ToString());

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
            //http://csharpsdk.org/docs/web/ajax-requests   
            //https://github.com/facebook-csharp-sdk/facebook-aspnet-sample/blob/master/src/facebook-aspnet-sample/Controllers/AccountController.cs
            //http://stackoverflow.com/questions/10187030/getting-accesstoken-from-code-using-facebook-c-sharp-sdk
            dynamic result = _facebookClient.Get("oauth/access_token", new {
                client_id = _facebookClient.AppId,
                client_secret = _facebookClient.AppSecret,
                redirect_uri = GenerateCallbackUri(),
                code = accesscode
            });

            return result.access_token;

            //var cl = new FacebookOAuthClient(FacebookClient);
            //cl.RedirectUri = 
            //cl.AppId = FacebookClient.AppId;
            //cl.AppSecret = FacebookClient.AppSecret;
            //var dict = (JsonObject)cl.ExchangeCodeForAccessToken(code, new Dictionary<string, object> { { "permissions", "offline_access" } });

            //return dict.Values.ElementAt(0).ToString();
        }
    }
}