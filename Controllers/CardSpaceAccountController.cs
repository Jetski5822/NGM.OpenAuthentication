using System.Web.Mvc;
using Orchard.Localization;
using Orchard.Themes;

namespace NGM.OpenAuthentication.Controllers {
    [Themed]
    public class CardSpaceAccountController : Controller {

        public CardSpaceAccountController() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            return DefaultLogOnResult();
        }

        private ActionResult DefaultLogOnResult(string returnUrl) {
            return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
        }
    }
}