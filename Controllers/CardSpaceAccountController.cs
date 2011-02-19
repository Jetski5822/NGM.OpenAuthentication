using System;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using DotNetOpenAuth.InfoCard;
using Orchard.Localization;
using Orchard.Themes;

namespace NGM.OpenAuthentication.Controllers {
    [Themed]
    [ContractVerification(true)]
    public class CardSpaceAccountController : Controller {

        public CardSpaceAccountController() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        [ValidateInput(false)]
        public ActionResult LogOn(string returnUrl) {
            throw new NotImplementedException("Not implemented just yet.");
            string xmlToken = this.HttpContext.Request.Params["xmlToken"];

            Token token = Token.Read(xmlToken);
            //token.
            //var fi token.Claims[ClaimTypes.GivenName];
            return DefaultLogOnResult(returnUrl);
        }

        private ActionResult DefaultLogOnResult(string returnUrl) {
            return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
        }
    }
}