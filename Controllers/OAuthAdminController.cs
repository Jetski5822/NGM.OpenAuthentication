using System.Linq;
using System.Web.Mvc;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.UI.Admin;

namespace NGM.OpenAuthentication.Controllers {
    [Admin]
    public class OAuthAdminController : Controller {
        private readonly IOrchardServices _orchardServices;

        public OAuthAdminController(IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
        }

        public ActionResult Index() {
            var viewModel = new OAuthAdminIndexViewModel {
                Providers = _orchardServices
                    .ContentManager
                    .Query<OAuthProviderSettingsPart, OAuthProviderSettingsPartRecord>()
                    .List()
                    .ToList()
            };

            return View("Index", viewModel);
        }

    }
}