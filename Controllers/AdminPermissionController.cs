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
        private readonly IOpenAuthenticationProviderPermissionService _openAuthenticationProviderPermissionService;

        public AdminPermissionController(IOpenAuthenticationProviderPermissionService openAuthenticationProviderPermissionService) {
            _openAuthenticationProviderPermissionService = openAuthenticationProviderPermissionService;
        }

        public ActionResult Edit() {
            var viewModel = new AdminProviderPermissionViewModel();
            var providerPermissions = _openAuthenticationProviderPermissionService.GetAll();
            viewModel.ProviderPermissions = providerPermissions
                .Select(o => o.Record.HashedProvider)
                .Distinct()
                .ToDictionary<int, string, IEnumerable<OpenAuthenticationPermissionSettingsPart>>
                    (ProviderHelpers.GetUserFriendlyStringForHashedProvider, provider => 
                        providerPermissions
                        .Where(o => o.Record.HashedProvider == provider)
                        .ToList());

            return View("Edit", viewModel);
        }
    }
}