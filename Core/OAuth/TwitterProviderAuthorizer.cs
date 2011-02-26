using System;
using System.Linq;
using LinqToTwitter;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class TwitterProviderAuthorizer : IOAuthProviderTwitterAuthorizer {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthorizer _authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        private readonly IOAuthCredentials _credentials;
        private readonly MvcAuthorizer _mvcAuthorizer;

        public TwitterProviderAuthorizer(IOrchardServices orchardServices,
            IAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authorizer = authorizer;
            _openAuthenticationService = openAuthenticationService;

            _credentials = new SessionStateCredentials {
                ConsumerKey = ClientKeyIdentifier,
                ConsumerSecret = ClientSecret
            };

            _mvcAuthorizer = new MvcAuthorizer { Credentials = _credentials };
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
            _mvcAuthorizer.CompleteAuthorization(GenerateCallbackUri());

            if (!_mvcAuthorizer.IsAuthorized) {
                _orchardServices.WorkContext.HttpContext.Session["knownProvider"] = Provider.ToString();

                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.RequresRedirect) { Result = _mvcAuthorizer.BeginAuthorization() };
            }

            _orchardServices.WorkContext.HttpContext.Session.Remove("knownProvider");

            var parameters = new OAuthAuthenticationParameters(Provider) {
                ExternalIdentifier = _mvcAuthorizer.OAuthTwitter.OAuthToken,
                ExternalDisplayIdentifier = _mvcAuthorizer.ScreenName,
                OAuthToken = _mvcAuthorizer.OAuthTwitter.OAuthToken,
                OAuthAccessToken = _mvcAuthorizer.OAuthTwitter.OAuthTokenSecret,
            };

            var status = _authorizer.Authorize(parameters);

            return new AuthorizeState(returnUrl, status) {
                Error = _authorizer.Error,
                RegisterModel = new RegisterModel(parameters) 
            };
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
            get { return OAuthProvider.Twitter; }
        }

        public ITwitterAuthorizer GetAuthorizer(IUser user) {
            var parameters = new OAuthAuthenticationParameters(Provider);
            var identifier = _openAuthenticationService.GetExternalIdentifiersFor(_orchardServices.WorkContext.CurrentUser).Where(o => o.HashedProvider == parameters.HashedProvider)
                .List()
                .FirstOrDefault();

            _mvcAuthorizer.Credentials.OAuthToken = identifier.Record.OAuthToken;
            _mvcAuthorizer.Credentials.AccessToken = identifier.Record.OAuthAccessToken;

            return _mvcAuthorizer;
        }
    }
}