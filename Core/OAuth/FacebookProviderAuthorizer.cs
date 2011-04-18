using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Facebook;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    [OrchardFeature("Facebook")]
    public class FacebookProviderAuthorizer : IOAuthProviderFacebookAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        private FacebookApplication _facebookApplication;

        public FacebookProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
            _openAuthenticationService = openAuthenticationService;
        }

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

        public AuthorizeState Authorize(string returnUrl) {
            FacebookOAuthResult oAuthResult;
            if (FacebookOAuthResult.TryParse(HttpContext.Current.Request.Url, out oAuthResult)) {
                return TranslateResponseState(returnUrl, oAuthResult);
            }

            return GenerateRequestState(returnUrl);
        }

        private AuthorizeState TranslateResponseState(string returnUrl, FacebookOAuthResult oAuthResult) {
            if (oAuthResult.IsSuccess) {
                var parameters = new OAuthAuthenticationParameters(Provider) {
                    ExternalIdentifier = GetAccessToken(oAuthResult.Code),
                    OAuthToken = oAuthResult.Code,
                    OAuthAccessToken = GetAccessToken(oAuthResult.Code)
                };

                var result = _authorizer.Authorize(parameters);

                if (result.Status == OpenAuthenticationStatus.AssociateOnLogon) {
                    if (_openAuthenticationService.GetSettings().Record.AutoRegisterEnabled)
                        result = GetUserNameAndRetryAuthorization(parameters);
                }

                return new AuthorizeState(returnUrl, result);
            }

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                Error = new KeyValuePair<string, string>("Provider", string.Format("Reason: {0}, Description: {1}", oAuthResult.ErrorReason, oAuthResult.ErrorDescription))
            };
        }

        private AuthorizationResult GetUserNameAndRetryAuthorization(OAuthAuthenticationParameters parameters) {
            var client = new FacebookClient(parameters.OAuthAccessToken);
            var me = client.Get("/me");

            var claimsTranslator = new FacebookClaimsTranslator();
            var claims = claimsTranslator.Translate((IDictionary<string, object>)me);

            parameters.AddClaim(claims);

            return _authorizer.Authorize(parameters);
        }

        private AuthorizeState GenerateRequestState(string returnUrl) {
            var facebookClient = new FacebookOAuthClient(FacebookApplication);
            
            var extendedPermissions = new[] { "publish_stream", "read_stream", "offline_access", "email" };
            var parameters = new Dictionary<string, object> {
                {"redirect_uri", GenerateCallbackUri() }
            };

            if (extendedPermissions != null && extendedPermissions.Length > 0) {
                var scope = new StringBuilder();
                scope.Append(string.Join(",", extendedPermissions));
                parameters["scope"] = scope.ToString();
            }

            var result = new RedirectResult(facebookClient.GetLoginUrl(parameters).ToString());

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.RequresRedirect) { Result = result };
        }

        private Uri GenerateCallbackUri() {
            UriBuilder builder = new UriBuilder(_orchardServices.WorkContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            var path = _orchardServices.WorkContext.HttpContext.Request.ApplicationPath + "/OAuth/LogOn/" + Provider.ToString();
            builder.Path = path.Replace(@"//", @"/");

            return builder.Uri;
        }

        public Provider Provider {
            get { return Provider.Facebook; }
        }

        public FacebookClient GetClient(IUser user) {
            var parameters = new OAuthAuthenticationParameters(Provider);
            var identifier = _openAuthenticationService.GetExternalIdentifiersFor(user).Where(o => o.HashedProvider == parameters.HashedProvider)
                .List()
                .FirstOrDefault();

            return !string.IsNullOrEmpty(identifier.Record.OAuthAccessToken) ? new FacebookClient(identifier.Record.OAuthAccessToken) : null;
        }

        private string GetAccessToken(string code) {
            FacebookOAuthClient cl = new FacebookOAuthClient(FacebookApplication);
            cl.RedirectUri = GenerateCallbackUri();
            cl.AppId = FacebookApplication.AppId;
            cl.AppSecret = FacebookApplication.AppSecret;
            Facebook.JsonObject dict = (Facebook.JsonObject)cl.ExchangeCodeForAccessToken(code, new Dictionary<string, object> { { "permissions", "offline_access" } });
            
            return dict.Values.ElementAt(0).ToString();
        }
    }
}