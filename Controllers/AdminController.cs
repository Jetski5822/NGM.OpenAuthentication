using System.Web.Mvc;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Controllers {
    [Admin]
    public class AdminController : Controller {
        private readonly IOrchardServices _orchardServices;
        private readonly IProviderConfigurationService _providerConfigurationService;

        public AdminController(IOrchardServices orchardServices,
            IProviderConfigurationService providerConfigurationService) {
            _orchardServices = orchardServices;
            _providerConfigurationService = providerConfigurationService;
        }

        public Localizer T { get; set; }

        public ActionResult Index() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage open authentication settings")))
                return new HttpUnauthorizedResult();

            var settings = _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>();

            var currentProviders = _providerConfigurationService.GetAll();
            
            var viewModel = new IndexViewModel {
                    AutoRegistrationEnabled = settings.AutoRegistrationEnabled,
                    CurrentProviders = currentProviders
                };

            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(IndexViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage open authentication settings")))
                return new HttpUnauthorizedResult();

            var settings = _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>();
            settings.AutoRegistrationEnabled = viewModel.AutoRegistrationEnabled;

            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage open authentication settings")))
                return new HttpUnauthorizedResult();

            _providerConfigurationService.Delete(id);

            return RedirectToAction("Index");
        }

        public ActionResult CreateProvider() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage open authentication settings")))
                return new HttpUnauthorizedResult();

            return View(new CreateProviderViewModel());
        }

        [HttpPost, ActionName("CreateProvider")]
        public ActionResult CreateProviderPost(CreateProviderViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage open authentication settings")))
                return new HttpUnauthorizedResult();

            if (!_providerConfigurationService.VerifyUnicity(viewModel.ProviderName)) {
                ModelState.AddModelError("ProviderName", T("Provider name already exists").ToString());
            }

            if (!_providerConfigurationService.VerifyUnicity(viewModel.ProviderName)) {
                ModelState.AddModelError("ProviderName", T("Provider name already exists").ToString());
            }

            if (!ModelState.IsValid) {
                _orchardServices.TransactionManager.Cancel();

                return View(viewModel);
            }

            _providerConfigurationService.Create(new ProviderConfigurationCreateParams {
                DisplayName = viewModel.DisplayName,
                ProviderName = viewModel.ProviderName,
                ProviderIdentifier = viewModel.ProviderIdentifier,
                ProviderIdKey = viewModel.ProviderIdKey,
                ProviderSecret = viewModel.ProviderSecret,
            });
            _orchardServices.Notifier.Information(T("Your configuration has been saved."));

            return RedirectToAction("Index");
        }
    }
}