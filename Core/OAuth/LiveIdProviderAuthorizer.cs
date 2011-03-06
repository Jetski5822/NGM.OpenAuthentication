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

        private WindowsLiveLogin _login;

        public LiveIdProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
            _openAuthenticationService = openAuthenticationService;

            _login = new WindowsLiveLogin(ClientKeyIdentifier, ClientSecret);
        }

        public AuthorizeState Authorize(string returnUrl) {
            if (_orchardServices.WorkContext.HttpContext.Request.Form["stoken"] != null) {
                return CompleteAuthorization(returnUrl);
            }
            
            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                Error = new KeyValuePair<string, string>("error", "Cookie not found.")
            };
        }

        private AuthorizeState CompleteAuthorization(string returnUrl) {
            WindowsLiveLogin.User user = _login.ProcessLogin(_orchardServices.WorkContext.HttpContext.Request.Form);

            if (user != null) {
                var parameters = new OAuthAuthenticationParameters(this.Provider) {
                    ExternalIdentifier = user.Id,
                    OAuthToken = user.Token
                };

                var status = _authorizer.Authorize(parameters);

                return new AuthorizeState(returnUrl, status) {
                    Error = _authorizer.Error,
                    RegisterModel = new RegisterModel(parameters)
                };
            }

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating);
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