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

            if (_orchardServices.WorkContext.HttpContext.Session == null)
                throw new NullReferenceException("Session is required.");

            if (!_mvcAuthorizer.IsAuthorized) {
                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.RequresRedirect) { Result = _mvcAuthorizer.BeginAuthorization() };
            }

            var parameters = new OAuthAuthenticationParameters(Provider) {
                ExternalIdentifier = _mvcAuthorizer.OAuthTwitter.OAuthToken,
                ExternalDisplayIdentifier = _mvcAuthorizer.ScreenName,
                OAuthToken = _mvcAuthorizer.OAuthTwitter.OAuthToken,
                OAuthAccessToken = _mvcAuthorizer.OAuthTwitter.OAuthTokenSecret,
            };

            var status = _authorizer.Authorize(parameters);

            var tempReturnUrl = _orchardServices.WorkContext.HttpContext.Request.QueryString["?ReturnUrl"];
            if (!string.IsNullOrEmpty(tempReturnUrl) && string.IsNullOrEmpty(returnUrl)) {
                returnUrl = tempReturnUrl;
            }

            return new AuthorizeState(returnUrl, status) {
                Error = _authorizer.Error,
                RegisterModel = new RegisterModel(parameters) 
            };
        }

        private Uri GenerateCallbackUri() {
            //var currentUrl = string.Empty;
            //if (_orchardServices.WorkContext.HttpContext.Request.Url != null)
            //    currentUrl = _orchardServices.WorkContext.HttpContext.Request.Url.ToString();

            //var seperator = "?";

            //if (currentUrl.Contains(seperator))
            //    seperator = "&";

            //if (!currentUrl.ToLowerInvariant().Contains("knownprovider="))
            //    currentUrl = string.Format("{0}{1}knownProvider={2}", currentUrl, seperator, Provider);

            //return new Uri(currentUrl);

            UriBuilder builder = new UriBuilder(_orchardServices.WorkContext.HttpContext.Request.Url);
            var path = _orchardServices.WorkContext.HttpContext.Request.ApplicationPath + "/OAuth/LogOn/" + Provider.ToString();
            builder.Path = path.Replace(@"//", @"/");

            return builder.Uri;
        }



        public OAuthProvider Provider {
            get { return OAuthProvider.Twitter; }
        }

        public ITwitterAuthorizer GetAuthorizer(IUser user) {
            var parameters = new OAuthAuthenticationParameters(Provider);
            var identifier = _openAuthenticationService.GetExternalIdentifiersFor(user).Where(o => o.HashedProvider == parameters.HashedProvider)
                .List()
                .FirstOrDefault();

            if (identifier != null) {
                _mvcAuthorizer.Credentials.OAuthToken = identifier.Record.OAuthToken;
                _mvcAuthorizer.Credentials.AccessToken = identifier.Record.OAuthAccessToken;

                return _mvcAuthorizer;
            }
            return null;
        }
    }
}