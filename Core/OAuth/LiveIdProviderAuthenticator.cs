using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Core.OAuth {
    [OrchardFeature("MicrosoftConnect")]
    public class LiveIdProviderAuthenticator : IOAuthProviderAuthenticator {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthenticator _authenticator;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public LiveIdProviderAuthenticator(IOrchardServices orchardServices,
            IAuthenticator authenticator,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _authenticator = authenticator;
            _openAuthenticationService = openAuthenticationService;
        }

        public AuthenticationState Authenticate(string returnUrl) {
            var request = _orchardServices.WorkContext.HttpContext.Request;
            var response = _orchardServices.WorkContext.HttpContext.Response;

            OAuthAuthenticationParameters parameters = new OAuthAuthenticationParameters(Provider);
            if (HasVerificationDetails()) {
                //http://msdn.microsoft.com/en-us/library/hh243641.aspx
            }

            var result = _authenticator.Authorize(parameters);

            var tempReturnUrl = _orchardServices.WorkContext.HttpContext.Request.QueryString["?ReturnUrl"];
            if (!string.IsNullOrEmpty(tempReturnUrl) && string.IsNullOrEmpty(returnUrl)) {
                returnUrl = tempReturnUrl;
            }

            return new AuthenticationState(returnUrl, result);
        }

        private bool HasVerificationDetails() {
            return ((VerificationCode() != null) && (VerificationState() != null));
        }

        private string VerificationCode() {
            return _orchardServices.WorkContext.HttpContext.Request.QueryString["code"];
        }

        private string VerificationState() {
            return _orchardServices.WorkContext.HttpContext.Request.QueryString["state"];
        }

        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientSecret; }
        }

        public bool IsConsumerConfigured {
            get { return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret); }
        }

        public Provider Provider {
            get { return Provider.LiveId; }
        }
    }
}