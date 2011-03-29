using System.Web.Mvc;

namespace NGM.OpenAuthentication.Controllers
{
    public class IndexController : Controller
    {
        public RedirectResult RemoveParameterAssociation(string returnUrl) {
            NGM.OpenAuthentication.Core.Authorizer.RemoveParameters();
            
            return Redirect(returnUrl);
        }
    }
}