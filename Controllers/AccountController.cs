using System.Web.Mvc;

namespace NGM.OpenAuthentication.Controllers
{
    public class AccountController : Controller {
        public ActionResult OpenIdLogOn() {
            return View();
        }

        [HttpPost, ActionName("OpenIdLogOn")]
        public ActionResult _OpenIdLogOn() {
            return new RedirectResult("https://www.google.com/accounts/ServiceLogin");
        }
    }
}
