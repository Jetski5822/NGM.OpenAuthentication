using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Security;

namespace NGM.OpenAuthentication.Controllers
{
    public class AccountController : Controller {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;
        }

        public ActionResult LogOn(string redirectUrl) {
            var relyingPartyWrapper = new OpenIdRelyingPartyWrapper();

            if (relyingPartyWrapper.HasResponse) {
                switch (relyingPartyWrapper.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var user = new OpenIdUser(relyingPartyWrapper.Response.FriendlyIdentifierForDisplay);
                        _authenticationService.SignIn(user, false);

                        return Redirect(!string.IsNullOrEmpty(redirectUrl) ? redirectUrl : "~/");
                    case AuthenticationStatus.Canceled:
                        ModelState.AddModelError("InvalidProvider", "Canceled at provider");
                        break;
                    case AuthenticationStatus.Failed:
                        ModelState.AddModelError("UnknownError", relyingPartyWrapper.Response.Exception.Message);
                        break;
                }
            }

            return View(new LogOnViewModel {RedirectUrl = redirectUrl});
        }

        [HttpPost]
        public ActionResult _LogOn(LogOnViewModel viewModel) {
            var relyingPartyWrapper = new OpenIdRelyingPartyWrapper();

            return BuildLogOnAuthenticationRedirect(relyingPartyWrapper, viewModel);
        }

        private ActionResult BuildLogOnAuthenticationRedirect(OpenIdRelyingPartyWrapper relyingPartyWrapper, LogOnViewModel viewModel) {
            var identifier = new OpenIdIdentifier(viewModel.OpenIdIdentifier);
            if (!identifier.IsValid) {
                ModelState.AddModelError("InvalidIdentifier", "Invalid Open ID identifier");
                return View(viewModel);
            }

            var request = relyingPartyWrapper.CreateRequest(identifier);
            return request.RedirectingResponse.AsActionResult();
        }
    }
}
