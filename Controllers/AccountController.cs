using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Security;

namespace NGM.OpenAuthentication.Controllers
{
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

                        var existingUser = _openAuthenticationService.GetUser(_openIdRelyingPartyService.Response.ClaimedIdentifier);

                        // If I am logged in, and another account has the identifier I am logging in with...
                        if (user != null && existingUser != null && !user.Equals(existingUser)) {
                            ModelState.AddModelError("IdentifierAssigned", "Identifier has already been assigned");
                            break;
                        }

                        // If I am not logged in, and I noone has this identifier, then go to register page to get them to confirm details.
                        if (user == null && existingUser == null) {
                            TempData["RegisterModel"] = new RegisterModel(_openIdRelyingPartyService.Response.ClaimedIdentifier);
                            return RedirectToAction("Register", "Account", new {area = "NGM.OpenAuthentication"});
                        }

                        // If I am logged in, and no user currently has that identifier.. then associate.
                        if (user != null && existingUser == null) {
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

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn(LogOnViewModel viewModel) {
            return BuildLogOnAuthenticationRedirect(viewModel);
        }

        public ActionResult Register() {
            var model = TempData["RegisterModel"] as RegisterModel;
            if (model == null)
                return RedirectToAction("LogOn", "Account", new {area = "NGM.OpenAuthentication"});

            return View("Register", new RegisterViewModel(model));
        }

        private ActionResult BuildLogOnAuthenticationRedirect(LogOnViewModel viewModel) {
            var identifier = new OpenIdIdentifier(viewModel.OpenIdIdentifier);
            if (!identifier.IsValid) {
                ModelState.AddModelError("OpenIdIdentifier", "Invalid Open ID identifier");
                return View(viewModel);
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);
                return request.RedirectingResponse.AsActionResult();
            }
            catch (ProtocolException ex) {
                ModelState.AddModelError("ProtocolException", string.Format("Unable to authenticate: {0}",ex.Message));
            }
            return View(viewModel);
        }
    }
}
    