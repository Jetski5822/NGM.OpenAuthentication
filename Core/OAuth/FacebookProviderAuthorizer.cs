using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Facebook;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class FacebookProviderAuthorizer : IOAuthProviderFacebookAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        private readonly FacebookApplication _facebookApplication;

        public FacebookProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
            _openAuthenticationService = openAuthenticationService;

            _facebookApplication = new FacebookApplication(ClientKeyIdentifier, ClientSecret);
        }

        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.FacebookClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.FacebookClientSecret; }
        }

        public bool IsConsumerConfigured {
            get {
                return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret);
            }
        }

        public AuthorizeState Authorize(string returnUrl) {
            FacebookOAuthResult oAuthResult;
            if (FacebookOAuthResult.TryParse(_orchardServices.WorkContext.HttpContext.Request.Url, out oAuthResult)) {
                return TranslateResponseState(returnUrl, oAuthResult);
            }
            return GenerateRequestState(returnUrl);
        }

        private AuthorizeState TranslateResponseState(string returnUrl, FacebookOAuthResult oAuthResult) {
            if (oAuthResult.IsSuccess) {
                var parameters = new OAuthAuthenticationParameters(Provider) {
                    ExternalIdentifier = oAuthResult.AccessToken,
                    OAuthToken = oAuthResult.AccessToken,
                };

                var status = _authorizer.Authorize(parameters);

                return new AuthorizeState(returnUrl, status) {
                    Error = _authorizer.Error,
                    RegisterModel = new RegisterModel(parameters)
                };
            }

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                Error = new KeyValuePair<string, string>("Provider", string.Format("Reason: {0}, Description: {1}", oAuthResult.ErrorReason, oAuthResult.ErrorDescription))
            };
        }

        private AuthorizeState GenerateRequestState(string returnUrl) {
            var facebookClient = new FacebookOAuthClient(_facebookApplication);

            var extendedPermissions = new[] { "publish_stream", "offline_access", "email" };
            var parameters = new Dictionary<string, object> {
                {"response_type", "token"},
                {"display", "popup"},
                {"redirect_uri", GenerateCallbackUri()}
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
            string currentUrl = _orchardServices.WorkContext.HttpContext.Request.Url.ToString();
            string seperator = "?";

            if (currentUrl.Contains(seperator))
                seperator = "&";

            if (!currentUrl.Contains("knownProvider="))
                currentUrl = string.Format("{0}{1}knownProvider={2}", currentUrl, seperator, Provider);

            return new Uri(currentUrl);
        }

        public OAuthProvider Provider {
            get { return OAuthProvider.Facebook; }
        }

        public FacebookClient GetClient(IUser user) {
            var parameters = new OAuthAuthenticationParameters(Provider);
            var identifier = _openAuthenticationService.GetExternalIdentifiersFor(user).Where(o => o.HashedProvider == parameters.HashedProvider)
                .List()
                .FirstOrDefault();

            return new FacebookClient(identifier.Record.OAuthToken);
        }
    }
}