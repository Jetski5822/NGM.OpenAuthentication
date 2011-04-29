using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.Core.Contents.Controllers;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;

namespace NGM.OpenAuthentication.Controllers {
    [Admin]
    public class AdminPermissionController : Controller {
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;

        public AdminPermissionController(IOrchardServices services, 
            IScopeProviderPermissionService scopeProviderPermissionService) {
            Services = services;
            _scopeProviderPermissionService = scopeProviderPermissionService;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Edit() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage scope permissions")))
                return new HttpUnauthorizedResult();

            var viewModel = new AdminProviderPermissionViewModel();
            var scopeProviderPermissions = _scopeProviderPermissionService.GetAll().ToList();

            var providerPermissions = new Dictionary<string, IEnumerable<ScopeProviderPermissionRecord>>();

            foreach (var hashedProvider in scopeProviderPermissions
                .Select(o => o.HashedProvider)
                .Distinct()
                .ToList()) {
                providerPermissions.Add(ProviderHelpers.GetUserFriendlyStringForHashedProvider(hashedProvider),
                    scopeProviderPermissions.Where(o => o.HashedProvider == hashedProvider).ToList()
                    );
            }

            viewModel.ProviderPermissions = providerPermissions;

            return View("Edit", viewModel);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditSavePOST() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage scope permissions")))
                return new HttpUnauthorizedResult();

            var viewModel = new AdminProviderPermissionViewModel();
            try {
                UpdateModel(viewModel);
                // Save
                Dictionary<int, bool> providerPermissions = new Dictionary<int, bool>();
                foreach (string key in Request.Form.Keys) {
                    if (key.StartsWith("Checkbox.")) {
                        var permissionId = int.Parse(key.Substring("Checkbox.".Length));
                        bool enabled = bool.Parse(Request.Form[key]);
                        providerPermissions.Add(permissionId, enabled);
                    }
                }
                _scopeProviderPermissionService.Update(providerPermissions);

                Services.Notifier.Information(T("Your Provider Permissions has been saved."));
            } catch (Exception exception) {
                this.Error(exception, T("Editing Provider Permissions failed: {0}", exception.Message), Logger, Services.Notifier);
            }
            return RedirectToAction("Edit");
        }
    }
}