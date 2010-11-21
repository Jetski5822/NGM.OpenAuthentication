using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Security;
using Orchard.Themes;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class AccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public AccountController(IOpenIdRelyingPartyService openIdRelyingPartyService, 
            IAuthenticationService authenticationService, 
            IOpenAuthenticationService openAuthenticationService) {
            
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
        }

        public ActionResult LogOn(string returnUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                // TODO : Not happy about this huge switch statement, consider a stratagy pattern possibly when I come to refactory?
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var user = _authenticationService.GetAuthenticatedUser();

                        bool isIdentifierAssigned = IsIdentifierAssigned(_openIdRelyingPartyService.Response.ClaimedIdentifier);

                        if (isIdentifierAssigned)
                            break;

                        // If I am not logged in, and I noone has this identifier, then go to register page to get them to confirm details.
                        if (user == null && !isIdentifierAssigned) {
                            var registerModelBuilder = new RegisterModelBuilder(_openIdRelyingPartyService.Response);
                            var model = registerModelBuilder.Build();
                            model.ReturnUrl = returnUrl;
                            TempData["RegisterModel"] = model;
                            return RedirectToAction("Register", "Account", new {area = "NGM.OpenAuthentication"});
                        }

                        // If I am logged in, and no user currently has that identifier.. then associate.
                        if (user != null && !isIdentifierAssigned) {
                            _openAuthenticationService.AssociateOpenIdWithUser(user, _openIdRelyingPartyService.Response.ClaimedIdentifier);

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

            // If I am logged in, and another account has the identifier I am logging in with...
            if (isIdentifierAssigned) {
                ModelState.AddModelError("IdentifierAssigned", "Identifier has already been assigned");
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

                viewModel = new RegisterViewModel(model);
            }

            return View("Register", viewModel);
        }

        [HttpPost, ActionName("Register")]
        public ActionResult _Register(RegisterViewModel viewModel) {
            if (ModelState.IsValid) {
                if (!_openAuthenticationService.IsAccountExists(viewModel.Model.Identifier)) {
                    var user = _openAuthenticationService.CreateUser(viewModel.Model);

                    // Sign In
                    _authenticationService.SignIn(user, false);

                    return Redirect(!string.IsNullOrEmpty(viewModel.Model.ReturnUrl) ? viewModel.Model.ReturnUrl : "~/");
                }
                ModelState.AddModelError("IdentifierAssigned", "Identifier has already been assigned");
            }
            return View("Register", viewModel);
        }

        public ActionResult VerifiedAccounts() {
            var user = _authenticationService.GetAuthenticatedUser();
            var identifiers = _openAuthenticationService.GetIdentifiersFor(user);
            var models = identifiers.Select(x => new AccountModel{Identifier = x, UserId = user.Id});

            return View("VerifiedAccounts", new VerifiedAccountsViewModel(models));
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