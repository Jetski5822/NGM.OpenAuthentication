using System;
using System.Linq;
using System.Threading;
using LinqToTwitter;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    [OrchardFeature("Twitter")]
    public class TwitterProviderAuthenticator : IOAuthProviderTwitterAuthenticator {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthenticator _authenticator;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        private IOAuthCredentials _credentials;
        private MvcAuthorizer _mvcAuthorizer;

        public TwitterProviderAuthenticator(IOrchardServices orchardServices,
            IAuthenticator authenticator,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authenticator = authenticator;
            _openAuthenticationService = openAuthenticationService;
        }

        private IOAuthCredentials Credentials {
            get {
                return _credentials ?? (_credentials = new SessionStateCredentials {
                    ConsumerKey = ClientKeyIdentifier,
                    ConsumerSecret = ClientSecret
                });
            }
        }

        private MvcAuthorizer MvcAuthorizer {
            get { return _mvcAuthorizer ?? (_mvcAuthorizer = new MvcAuthorizer { Credentials = this.Credentials }); }
        }

        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.TwitterClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.TwitterClientSecret; }
        }

        public bool IsConsumerConfigured {
            get { return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret); }
        }

        public AuthenticationState Authenticate(string returnUrl) {
            // Sleep for 15 seconds as a workaround for a twitter bug. :(
            Thread.Sleep(new TimeSpan(0, 0, 0, 15));
            MvcAuthorizer.CompleteAuthorization(GenerateCallbackUri());

            if (_orchardServices.WorkContext.HttpContext.Session == null)
                throw new NullReferenceException("Session is required.");

            if (!MvcAuthorizer.IsAuthorized) {
                return new AuthenticationState(returnUrl, OpenAuthenticationStatus.RequresRedirect) { Result = MvcAuthorizer.BeginAuthorization() };
            }

            var parameters = new OAuthAuthenticationParameters(Provider) {
                ExternalIdentifier = MvcAuthorizer.OAuthTwitter.OAuthToken,
                ExternalDisplayIdentifier = MvcAuthorizer.ScreenName,
                OAuthToken = MvcAuthorizer.OAuthTwitter.OAuthToken,
                OAuthAccessToken = MvcAuthorizer.OAuthTwitter.OAuthTokenSecret,
            };

            var result = _authenticator.Authorize(parameters);

            var tempReturnUrl = _orchardServices.WorkContext.HttpContext.Request.QueryString["?ReturnUrl"];
            if (!string.IsNullOrEmpty(tempReturnUrl) && string.IsNullOrEmpty(returnUrl)) {
                returnUrl = tempReturnUrl;
            }

            return new AuthenticationState(returnUrl, result);
        }

        private Uri GenerateCallbackUri() {
            UriBuilder builder = new UriBuilder(_orchardServices.WorkContext.HttpContext.Request.Url);
            var path = _orchardServices.WorkContext.HttpContext.Request.ApplicationPath + "/OAuth/LogOn/" + Provider.ToString();
            builder.Path = path.Replace(@"//", @"/");
            builder.Query = builder.Query.Replace(@"??", @"?");

            return builder.Uri;
        }

        public Provider Provider {
            get { return Provider.Twitter; }
        }

        public ITwitterAuthorizer GetAuthorizer(IUser user) {
            var parameters = new OAuthAuthenticationParameters(Provider);
            var identifier = _openAuthenticationService.GetExternalIdentifiersFor(user).Where(o => o.HashedProvider == parameters.HashedProvider)
                .List()
                .FirstOrDefault();

            if (identifier != null) {
                MvcAuthorizer.Credentials.OAuthToken = identifier.Record.OAuthToken;
                MvcAuthorizer.Credentials.AccessToken = identifier.Record.OAuthAccessToken;

                return MvcAuthorizer;
            }
            return null;
        }
    }
}