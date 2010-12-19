using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class AccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOrchardServices _orchardServices;

        public AccountController(
            IOpenIdRelyingPartyService openIdRelyingPartyService, 
            IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            IOrchardServices orchardServices)
        {
            
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _orchardServices = orchardServices;
        }

        public ActionResult LogOn(string returnUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                // TODO : Not happy about this huge switch statement, consider a stratagy pattern possibly when I come to refactory?
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var user = _authenticationService.GetAuthenticatedUser();

                        bool isClaimedIdentifierAssigned = IsIdentifierAssigned(_openIdRelyingPartyService.Response.ClaimedIdentifier);

                        if (isClaimedIdentifierAssigned)
                            break;

                        // If I am not logged in, and I noone has this identifier, then go to register page to get them to confirm details.
                        if (user == null && !isClaimedIdentifierAssigned) {
                            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();

                            if ((registrationSettings != null) &&
                                (registrationSettings.UsersCanRegister == true))
                            {
                                ModelState.AddModelError("AccessDenied", "User registration is disabled");
                                break;
                            }
                            else
                            {
                                var registerModelBuilder = new RegisterModelBuilder(_openIdRelyingPartyService.Response);
                                var model = registerModelBuilder.Build();
                                model.ReturnUrl = returnUrl;

                                TempData["RegisterModel"] = model;
                                return RedirectToAction("Register", "Account", new { area = "NGM.OpenAuthentication" });
                            }
                        }

                        // If I am logged in, and no user currently has that identifier.. then associate.
                        if (user != null && !isClaimedIdentifierAssigned) {
                            _openAuthenticationService.AssociateOpenIdWithUser(user, _openIdRelyingPartyService.Response.ClaimedIdentifier, _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay);

                            _authenticationService.SignIn(user, false);
                        }

                        return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
                    case AuthenticationStatus.Canceled:
                        ModelState.AddModelError("InvalidProvider", "Canceled at provider");
                        break;
                    case AuthenticationStatus.Failed:
                        ModelState.AddModelError("UnknownError", _openIdRelyingPartyService.Response.Exception.Message);
                        break;
                }
            }

            return View("LogOn", new LogOnViewModel { ReturnUrl = returnUrl });
        }

        private bool IsIdentifierAssigned(string identifier) {
            var isIdentifierAssigned = _openAuthenticationService.IsAccountExists(identifier);

            // Check to see if identifier is currently assigned.
            if (isIdentifierAssigned) {
                ModelState.AddModelError("IdentifierAssigned", "ClaimedIdentifier has already been assigned");
            }
            return isIdentifierAssigned;
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn(LogOnViewModel viewModel) {
            if (IsIdentifierAssigned(viewModel.OpenIdIdentifier))
                return View("LogOn", viewModel);

            return BuildLogOnAuthenticationRedirect(viewModel);
        }

        public ActionResult Register(RegisterViewModel viewModel) {
            if (viewModel == null || viewModel.Model == null) {
                var model = TempData["RegisterModel"] as RegisterModel;
                if (model == null)
                    return RedirectToAction("LogOn", "Account", new {area = "NGM.OpenAuthentication"});

                viewModel = new RegisterViewModel {Model = model};
            }

            return View("Register", viewModel);
        }

        [HttpPost, ActionName("Register")]
        public ActionResult _Register(RegisterViewModel viewModel) {
            if (ModelState.IsValid) {
                if (!_openAuthenticationService.IsAccountExists(viewModel.Model.ClaimedIdentifier)) {
                    var user = _openAuthenticationService.CreateUser(viewModel.Model);

                    // Sign In
                    _authenticationService.SignIn(user, false);

                    return Redirect(!string.IsNullOrEmpty(viewModel.Model.ReturnUrl) ? viewModel.Model.ReturnUrl : "~/");
                }
                ModelState.AddModelError("IdentifierAssigned", "ClaimedIdentifier has already been assigned");
            }
            return View("Register", viewModel);
        }

        public ActionResult VerifiedAccounts() {
            var user = _authenticationService.GetAuthenticatedUser();
            var entries =
                _openAuthenticationService
                    .GetIdentifiersFor(user)
                    .List()
                    .ToList()
                    .Select(account => CreateAccountEntry(account.Record));

            var viewModel = new VerifiedAccountsViewModel {
                Accounts = entries.ToList(), 
                UserId = user.Id
            };

            return View("VerifiedAccounts", viewModel);
        }

        private AccountEntry CreateAccountEntry(OpenAuthenticationPartRecord openAuthenticationPart) {
            return new AccountEntry {
                Account = openAuthenticationPart
            };
        }

        [HttpPost, ActionName("VerifiedAccounts")]
        public ActionResult _VerifiedAccounts(FormCollection input) {
            var viewModel = new VerifiedAccountsViewModel {Accounts = new List<AccountEntry>()};
            UpdateModel(viewModel, input.ToValueProvider());

            foreach (var accountEntry in viewModel.Accounts.Where(c => c.IsChecked)) {
                _openAuthenticationService.RemoveOpenIdAssociation(accountEntry.Account.ClaimedIdentifier);
            }

            return RedirectToRoute("VerifiedAccounts");
        }

        private ActionResult BuildLogOnAuthenticationRedirect(LogOnViewModel viewModel) {
            var identifier = new OpenIdIdentifier(viewModel.OpenIdIdentifier);
            if (!identifier.IsValid) {
                ModelState.AddModelError("OpenIdIdentifier", "Invalid Open ID identifier");
                return View(viewModel);
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);
                
                request.AddExtension(Claims.CreateRequest(_openAuthenticationService.GetSettings()));

                return request.RedirectingResponse.AsActionResult();
            }
            catch (ProtocolException ex) {
                ModelState.AddModelError("ProtocolException", string.Format("Unable to authenticate: {0}",ex.Message));
            }
            return View(viewModel);
        }
    }
}