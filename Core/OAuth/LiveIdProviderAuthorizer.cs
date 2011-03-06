using System.Collections.Generic;
using System.Web;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using WindowsLive;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class LiveIdProviderAuthorizer : IOAuthProviderAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        const string LoginCookie = "webauthtoken";

        public LiveIdProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
            _openAuthenticationService = openAuthenticationService;
        }

        public AuthorizeState Authorize(string returnUrl) {
            var cookie = GetCookie();
            if (cookie != null) {
                var login = new WindowsLiveLogin(ClientKeyIdentifier, ClientSecret);
                return CompleteAuthorization(returnUrl, cookie, login);
            }

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                Error = new KeyValuePair<string, string>("error", "Cookie not found.")
            };
        }

        private AuthorizeState CompleteAuthorization(string returnUrl, HttpCookie loginCookie, WindowsLiveLogin login) {
            string token = loginCookie.Value;

            if (!string.IsNullOrEmpty(token)) {
                WindowsLiveLogin.User user = login.ProcessToken(token);

                if (user != null) {
                    var parameters = new OAuthAuthenticationParameters(this.Provider) {
                        ExternalIdentifier = user.Id,
                        ExternalDisplayIdentifier = user.Id,
                        OAuthToken = user.Token
                    };

                    var status = _authorizer.Authorize(parameters);

                    return new AuthorizeState(returnUrl, status) {
                        Error = _authorizer.Error,
                        RegisterModel = new RegisterModel(parameters)
                    };
                }
            }
            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating);
        }

        private HttpCookie GetCookie() {
            return _orchardServices.WorkContext.HttpContext.Request.Cookies[LoginCookie];
        }

        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientSecret; }
        }

        public bool IsConsumerConfigured {
            get {
                return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret);
            }
        }

        public OAuthProvider Provider {
            get { return OAuthProvider.LiveId; }
        }
    }
}