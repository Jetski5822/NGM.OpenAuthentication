using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Facebook;
using NGM.OpenAuthentication.Services;
using Orchard;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class FacebookProviderAuthorizer : IOAuthProviderAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        private readonly FacebookApplication _facebookApplication;

        public FacebookProviderAuthorizer(IOrchardServices orchardServices,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
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
            var facebookClient = new FacebookOAuthClient(_facebookApplication);

            var extendedPermissions = new[] { "publish_stream", "offline_access", "email" };
            var parameters = new Dictionary<string, object>
                    {
                        { "response_type", "token" },
                        { "display", "popup" },
                        { "redirect_uri", GenerateCallbackUri() }
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
    }
}