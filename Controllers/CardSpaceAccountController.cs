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
            return View();
        }
    }
}