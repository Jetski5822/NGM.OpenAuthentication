using System;
using LinqToTwitter;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class TwitterAuthorizer : IOAuthAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IOpenAuthorizer _openAuthorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        private readonly IOAuthCredentials _credentials;
        private readonly MvcAuthorizer _authorizer;

        public TwitterAuthorizer(IOrchardServices orchardServices,
            IOpenAuthorizer openAuthorizer,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _openAuthorizer = openAuthorizer;
            _openAuthenticationService = openAuthenticationService;

            _credentials = new SessionStateCredentials {
                ConsumerKey = ClientKeyIdentifier,
                ConsumerSecret = ClientSecret
            };

            _authorizer = new MvcAuthorizer {Credentials = _credentials};
        }

        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.TwitterClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.TwitterClientSecret; }
        }

        public bool IsConsumerConfigured {
            get {
                return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret);
            }
        }

        public AuthorizeState Authorize(string returnUrl) {
            _authorizer.CompleteAuthorization(GenerateReturnUri());
            
            if (!_authorizer.IsAuthorized) {
                _orchardServices.WorkContext.HttpContext.Session["knownProvider"] = Provider.ToString();

                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.RequresRedirect) { Result = _authorizer.BeginAuthorization() };
            }

            _orchardServices.WorkContext.HttpContext.Session.Remove("knownProvider");

            var status = _openAuthorizer.Authorize(_authorizer.UserId, _authorizer.ScreenName);

            return new AuthorizeState(returnUrl, status) {
                Error = _openAuthorizer.Error,
                RegisterModel = new RegisterModel {
                    ExternalIdentifier = _authorizer.OAuthTwitter.OAuthToken, 
                    ExternalDisplayIdentifier = _authorizer.ScreenName, 
                    UserName = _authorizer.ScreenName, 
                    Provider = Provider.ToString()
                }
            };
        }

        private Uri GenerateReturnUri() {
            string currentUrl = _orchardServices.WorkContext.HttpContext.Request.Url.ToString();
            string seperator = "?";

            if (currentUrl.Contains(seperator))
                seperator = "&";

            if (!currentUrl.Contains("knownProvider="))
                currentUrl = string.Format("{0}{1}knownProvider={2}", currentUrl, seperator, Provider);

            return new Uri(currentUrl);
        }

        public OAuthProvider Provider {
            get { return OAuthProvider.Twitter; }
        }
    }
}