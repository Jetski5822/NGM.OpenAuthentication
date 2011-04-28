using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.UI.Admin;

namespace NGM.OpenAuthentication.Controllers {
    [Admin]
    public class AdminPermissionController : Controller {
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;

        public AdminPermissionController(IScopeProviderPermissionService scopeProviderPermissionService) {
            _scopeProviderPermissionService = scopeProviderPermissionService;
        }

        public ActionResult Edit() {
            var viewModel = new AdminProviderPermissionViewModel();
            var providerPermissions = _scopeProviderPermissionService.GetAll();
            viewModel.ProviderPermissions = providerPermissions
                .Select(o => o.HashedProvider)
                .Distinct()
                .ToDictionary<int, string, IEnumerable<ScopeProviderPermissionRecord>>
                    (ProviderHelpers.GetUserFriendlyStringForHashedProvider, provider => 
                        providerPermissions
                        .Where(o => o.HashedProvider == provider)
                        .ToList());

            return View("Edit", viewModel);
        }
    }
}