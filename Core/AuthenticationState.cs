using System.Web.Mvc;

namespace NGM.OpenAuthentication.Core {
    public class AuthenticationState {
        public AuthenticationState(string returnUrl, Statuses statuses) {
            Status = statuses;

            if (Status == Statuses.Authenticated)
                Result = new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
        }

        public Statuses Status { get; private set; }

        public ActionResult Result { get; set; }
    }
}