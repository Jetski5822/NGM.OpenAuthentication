using System.Web.Mvc;
using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Controllers
{
    public class IndexController : Controller
    {
        public RedirectResult RemoveParameterAssociation(string returnUrl) {
            State.Clear();
            
            return Redirect(returnUrl);
        }
    }
}