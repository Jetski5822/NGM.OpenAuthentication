using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NGM.OpenAuthentication.Models;
using Orchard;
using WindowsLive;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class LiveIdProviderAuthorizer : IOAuthProviderAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;

        const string LoginCookie = "webauthtoken";

        public LiveIdProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
        }

        public AuthorizeState Authorize(string returnUrl) {
            var login = new WindowsLiveLogin(ClientKeyIdentifier, ClientSecret);

            var cookie = GetCookie();
            if (cookie != null) {
                return CompleteAuthorization(returnUrl, cookie, login);
            }

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating);
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
            get { throw new NotImplementedException(); }
        }

        public string ClientSecret {
            get { throw new NotImplementedException(); }
        }

        public bool IsConsumerConfigured {
            get { throw new NotImplementedException(); }
        }

        public OAuthProvider Provider {
            get { return OAuthProvider.LiveId; }
        }
    }
}