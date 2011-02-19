using System.Collections.Generic;
using System.Web.Mvc;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class AuthorizeState {
        private readonly string _returnUrl;

        public AuthorizeState(string returnUrl, OpenAuthenticationStatus authenticationStatus) {
            _returnUrl = returnUrl;
            AuthenticationStatus = authenticationStatus;

            if (authenticationStatus == OpenAuthenticationStatus.Authenticated)
                Result = new RedirectResult(!string.IsNullOrEmpty(_returnUrl) ? _returnUrl : "~/");
        }

        public OpenAuthenticationStatus AuthenticationStatus { get; private set; }

        public KeyValuePair<string, string> Error { get; set; }

        public RegisterModel RegisterModel { get; set; }

        public ActionResult Result { get; set; }
    }
}