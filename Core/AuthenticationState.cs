using System.Collections.Generic;
using System.Web.Mvc;

namespace NGM.OpenAuthentication.Core {
    public class AuthenticationState {
        public AuthenticationState(string returnUrl, OpenAuthenticationStatus openAuthenticationStatus) {
            Status = openAuthenticationStatus;

            if (Status == OpenAuthenticationStatus.Authenticated)
                Result = new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
        }

        public AuthenticationState(string returnUrl, AuthenticationResult authenticationResult) : this (returnUrl, authenticationResult.Status) {
            Error = authenticationResult.Error;
        }

        public OpenAuthenticationStatus Status { get; private set; }

        public KeyValuePair<string, string> Error { get; set; }

        public ActionResult Result { get; set; }
    }
}