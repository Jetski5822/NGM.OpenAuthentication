using System.Web.Mvc;
using System.Web.UI.WebControls;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Security;

namespace NGM.OpenAuthentication.Controllers
{
    public class AccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IOpenIdRelyingPartyService openIdRelyingPartyService, IAuthenticationService authenticationService) {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authenticationService = authenticationService;
        }

        public ActionResult LogOn(string redirectUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var user = new OpenIdUser(_openIdRelyingPartyService.Response.ClaimedIdentifier);
                        _authenticationService.SignIn(user, false);

                        return Redirect(!string.IsNullOrEmpty(redirectUrl) ? redirectUrl : "~/");
                    case AuthenticationStatus.Canceled:
                        ModelState.AddModelError("InvalidProvider", "Canceled at provider");
                        break;
                    case AuthenticationStatus.Failed:
                        ModelState.AddModelError("UnknownError", _openIdRelyingPartyService.Response.Exception.Message);
                        break;
                }
            }

            return View("LogOn", new LogOnViewModel {RedirectUrl = redirectUrl});
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult LogOn(LogOnViewModel viewModel) {
            return BuildLogOnAuthenticationRedirect(viewModel);
        }

        public ActionResult Register(string redirectUrl) {
            return null;
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
