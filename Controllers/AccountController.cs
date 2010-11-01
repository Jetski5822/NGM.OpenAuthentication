using System;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Security;

namespace NGM.OpenAuthentication.Controllers
{
    public class AccountController : Controller {
        private readonly IAuthenticationResolverService _authenticationResolverService;

        public AccountController(IAuthenticationResolverService authenticationResolverService) {
            _authenticationResolverService = authenticationResolverService;
        }

        public ActionResult LogOn(string redirectUrl) {
            var relyingPartyWrapper = new OpenIdRelyingPartyWrapper();

            if (relyingPartyWrapper.HasResponse) {
                switch (relyingPartyWrapper.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        if (_authenticationResolverService.IsAccountValidFor(relyingPartyWrapper.Response))
                            _authenticationResolverService.AuthenticateResponse(relyingPartyWrapper.Response);

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

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn(LogOnViewModel viewModel) {
            var relyingPartyWrapper = new OpenIdRelyingPartyWrapper();

            return BuildLogOnAuthenticationRedirect(relyingPartyWrapper, viewModel);
        }

        private ActionResult BuildLogOnAuthenticationRedirect(OpenIdRelyingPartyWrapper relyingPartyWrapper, LogOnViewModel viewModel) {
            var identifier = new OpenIdIdentifier(viewModel.OpenIdIdentifier);
            if (!identifier.IsValid) {
                ModelState.AddModelError("OpenIdIdentifier", "Invalid Open ID identifier");
                return View(viewModel);
            }

            try {
                var request = relyingPartyWrapper.CreateRequest(identifier);
                return request.RedirectingResponse.AsActionResult();
            }
            catch (ProtocolException ex) {
                ModelState.AddModelError("ProtocolException", string.Format("Unable to authenticate: {0}",ex.Message));
            }
            return View(viewModel);
        }
    }
}
