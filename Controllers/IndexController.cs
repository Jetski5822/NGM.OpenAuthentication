using System.Web.Mvc;
using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Controllers
{
    public class IndexController : Controller
    {
        private readonly IStateBag _stateBag;

        public IndexController(IStateBag stateBag) {
            _stateBag = stateBag;
        }

        public RedirectResult RemoveParameterAssociation(string returnUrl) {
            _stateBag.Clear();
            
            return Redirect(returnUrl);
        }
    }
}