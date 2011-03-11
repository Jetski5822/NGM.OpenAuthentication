using System.Collections.Generic;
using System.Web.Mvc;

namespace NGM.OpenAuthentication.Core {
    public class AuthorizeState {
        private readonly string _returnUrl;

        public AuthorizeState(string returnUrl, OpenAuthenticationStatus openAuthenticationStatus) {
            _returnUrl = returnUrl;
            AuthenticationStatus = openAuthenticationStatus;

            if (AuthenticationStatus == OpenAuthenticationStatus.Authenticated)
                Result = new RedirectResult(!string.IsNullOrEmpty(_returnUrl) ? _returnUrl : "~/");
        }

        public AuthorizeState(string returnUrl, AuthorizationResult authorizationResult) : this (returnUrl, authorizationResult.Status) {
            Error = authorizationResult.Error;
        }

        public OpenAuthenticationStatus AuthenticationStatus { get; private set; }

        public KeyValuePair<string, string> Error { get; set; }

        public ActionResult Result { get; set; }
    }
}