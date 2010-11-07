using System.Web.Mvc;
using System.Web.UI.WebControls;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Security;

namespace NGM.OpenAuthentication.Controllers
{
    public class AccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public AccountController(IOpenIdRelyingPartyService openIdRelyingPartyService, IAuthenticationService authenticationService, IOpenAuthenticationService openAuthenticationService) {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
        }

        public ActionResult LogOn(string redirectUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var user = _authenticationService.GetAuthenticatedUser();
                        
                        if (user == null) {
                            user = _openAuthenticationService.CreateUser(_openIdRelyingPartyService.Response.ClaimedIdentifier);

                            _authenticationService.SignIn(user, false);
                        }
                        
                        _openAuthenticationService.AssociateOpenIdWithUser(user, _openIdRelyingPartyService.Response.ClaimedIdentifier);

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
        public ActionResult _LogOn(LogOnViewModel viewModel) {
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
