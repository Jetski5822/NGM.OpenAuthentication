using System;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
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
            //string xmlToken = this.HttpContext.Request.Params["xmlToken"];

            //Token token = Token.Read(xmlToken);
            ////token.
            ////var fi token.Claims[ClaimTypes.GivenName];
            //return new RedirectResult(Url.Referer(this.Request));
        }
    }
}