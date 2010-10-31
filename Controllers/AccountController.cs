using System.Web.Mvc;

namespace NGM.OpenAuthentication.Controllers
{
    public class AccountController : Controller {
        public ActionResult LogOn() {
            return View();
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn() {
            return null;
        }
    }
}
