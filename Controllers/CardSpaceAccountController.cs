using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Web.Handlers;
using System.Web.Mvc;
using System.Web.UI;
using DotNetOpenAuth.InfoCard;
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
            return DefaultLogOnResult(returnUrl);
        }

        private ActionResult DefaultLogOnResult(string returnUrl) {
            return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
        }
    }
}