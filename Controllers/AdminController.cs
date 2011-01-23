using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Notify;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Controllers {
    public class AdminController : Controller {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IOrchardServices _orchardServices;

        public AdminController(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            IOpenIdRelyingPartyService openIdRelyingPartyService,
            IOrchardServices orchardServices) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

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

        public ActionResult Create(string returnUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                // TODO : Not happy about this huge switch statement, consider a stratagy pattern possibly when I come to refactory?
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var user = _authenticationService.GetAuthenticatedUser();

                        if (user == null)
                            break;

                        bool isClaimedIdentifierAssigned = IsIdentifierAssigned(_openIdRelyingPartyService.Response.ClaimedIdentifier);

                        if (isClaimedIdentifierAssigned)
                            break;

                        // If I am logged in, and no user currently has that identifier.. then associate.
                        _openAuthenticationService.AssociateOpenIdWithUser(user, _openIdRelyingPartyService.Response.ClaimedIdentifier, _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay);

                        _orchardServices.Notifier.Information(T("OpenID succesfully associated to logged in account"));

                        return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
                    case AuthenticationStatus.Canceled:
                        AddError("Canceled at provider");
                        break;
                    case AuthenticationStatus.Failed:
                        AddError(_openIdRelyingPartyService.Response.Exception.Message);
                        break;
                }
            }
            
            return View("Create");
        }

        [HttpPost, ActionName("Create")]
        public ActionResult _Create(FormCollection formCollection) {
            var viewModel = new CreateViewModel();
            TryUpdateModel(viewModel, formCollection);

            var identifier = new OpenIdIdentifier(viewModel.OpenIdIdentifier);
            if (!identifier.IsValid) {
                AddError("Invalid Open ID identifier");
            } else {
                try {
                    var request = _openIdRelyingPartyService.CreateRequest(identifier);

                    request.AddExtension(Claims.CreateRequest(_openAuthenticationService.GetSettings()));

                    return request.RedirectingResponse.AsActionResult();
                }
                catch (ProtocolException ex) {
                    AddError(string.Format("Unable to authenticate: {0}", ex.Message));
                }
            }
            return View("Create", viewModel);
        }


        [HttpPost]
        public ActionResult Delete(string claimedIdentifier, string returnUrl) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage OpenID")))
                return new HttpUnauthorizedResult();

            try {
                _openAuthenticationService.RemoveOpenIdAssociation(claimedIdentifier);

                _orchardServices.Notifier.Information(T("OpenID was successfully deleted."));
            } catch (Exception exception) {
                _orchardServices.Notifier.Error(T("Editing OpenID failed: {0}", exception.Message));
            }
            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        private AccountEntry CreateAccountEntry(OpenAuthenticationPartRecord openAuthenticationPart) {
            return new AccountEntry {
                Account = openAuthenticationPart
            };
        }

        private bool IsIdentifierAssigned(string identifier) {
            var isIdentifierAssigned = _openAuthenticationService.IsAccountExists(identifier);

            // Check to see if identifier is currently assigned.
            if (isIdentifierAssigned) {
                AddError("ClaimedIdentifier has already been assigned");
            }
            return isIdentifierAssigned;
        }

        private void AddError(string value) {
            _orchardServices.Notifier.Error(T(value));
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