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
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Controllers {
    [Admin]
    public class AdminController : Controller {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOrchardServices _orchardServices;

        public AdminController(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            IOrchardServices orchardServices) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult Index() {
            var user = _authenticationService.GetAuthenticatedUser();
            var entries =
                _openAuthenticationService
                    .GetExternalIdentifiersFor(user)
                    .List()
                    .ToList()
                    .Select(account => CreateAccountEntry(account.Record));

            var viewModel = new AdminIndexViewModel {
                Accounts = entries.ToList(),
                Options = new AdminIndexOptions()
            };

            return View("Index", viewModel);
        }

        [HttpPost]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult Index(FormCollection input) {
            var viewModel = new AdminIndexViewModel { Accounts = new List<AccountEntry>() };
            UpdateModel(viewModel, input);

            var checkedEntries = viewModel.Accounts.Where(c => c.IsChecked);
            switch (viewModel.Options.BulkAction) {
                case AdminBulkAction.None:
                    break;
                case AdminBulkAction.Delete:
                    foreach (var entry in checkedEntries) {
                        RemoveAccountAssociation(new HashedOpenAuthenticationParameters(entry.Account.HashedProvider, entry.Account.ExternalIdentifier));
                    }
                    break;
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Create(string returnUrl) {
            return View("Create");
        }

        [HttpPost]
        public ActionResult Delete(string externalIdentifier, string returnUrl, int? hashedProvider) {
            RemoveAccountAssociation(new HashedOpenAuthenticationParameters(hashedProvider.GetValueOrDefault(), externalIdentifier));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        private void RemoveAccountAssociation(OpenAuthenticationParameters parameters) {
            try {
                _openAuthenticationService.RemoveAssociation(parameters);

                _orchardServices.Notifier.Information(T("Account was successfully deleted."));
            } catch (Exception exception) {
                _orchardServices.Notifier.Error(T("Editing Account failed: {0}", exception.Message));
            }
        }

        private static AccountEntry CreateAccountEntry(OpenAuthenticationPartRecord openAuthenticationPart) {
            return new AccountEntry {
                Account = openAuthenticationPart
            };
        }
    }
}