using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Security;

namespace NGM.OpenAuthentication.Controllers {
    public class AdminController : Controller {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public AdminController(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
        }

        public ActionResult Index() {
            var user = _authenticationService.GetAuthenticatedUser();
            var entries =
                _openAuthenticationService
                    .GetIdentifiersFor(user)
                    .List()
                    .ToList()
                    .Select(account => CreateAccountEntry(account.Record));

            var viewModel = new IndexViewModel {
                Accounts = entries.ToList(),
                UserId = user.Id
            };

            return View("Index", viewModel);
        }

        private AccountEntry CreateAccountEntry(OpenAuthenticationPartRecord openAuthenticationPart) {
            return new AccountEntry {
                Account = openAuthenticationPart
            };
        }

        //[HttpPost, ActionName("VerifiedAccounts")]
        //public ActionResult _VerifiedAccounts(FormCollection input) {
        //    var viewModel = new VerifiedAccountsViewModel {Accounts = new List<AccountEntry>()};
        //    UpdateModel(viewModel, input.ToValueProvider());

        //    foreach (var accountEntry in viewModel.Accounts.Where(c => c.IsChecked)) {
        //        _openAuthenticationService.RemoveOpenIdAssociation(accountEntry.Account.ClaimedIdentifier);
        //    }

        //    return RedirectToRoute("VerifiedAccounts");
        //}
    }
}