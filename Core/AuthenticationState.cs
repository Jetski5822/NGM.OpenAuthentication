using System.Collections.Generic;
using System.Web.Mvc;

namespace NGM.OpenAuthentication.Core {
    public class AuthenticationState {
        private readonly string _returnUrl;

        public AuthenticationState(string returnUrl, OpenAuthenticationStatus openAuthenticationStatus) {
            _returnUrl = returnUrl;
            AuthenticationStatus = openAuthenticationStatus;

            if (AuthenticationStatus == OpenAuthenticationStatus.Authenticated)
                Result = new RedirectResult(!string.IsNullOrEmpty(_returnUrl) ? _returnUrl : "~/");
        }

        public AuthenticationState(string returnUrl, AuthenticationResult authenticationResult) : this (returnUrl, authenticationResult.Status) {
            Error = authenticationResult.Error;
        }

        public OpenAuthenticationStatus AuthenticationStatus { get; private set; }

        public KeyValuePair<string, string> Error { get; set; }

        public ActionResult Result { get; set; }
    }
}