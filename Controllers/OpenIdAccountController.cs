using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Localization;
using Orchard.Themes;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class OpenIdAccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOpenAuthorizer _openAuthorizer;

        public OpenIdAccountController(IOpenIdRelyingPartyService openIdRelyingPartyService, IOpenAuthenticationService openAuthenticationService, IOpenAuthorizer openAuthorizer)
        {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _openAuthenticationService = openAuthenticationService;
            _openAuthorizer = openAuthorizer;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                // TODO : Not happy about this huge switch statement, consider a stratagy pattern possibly when I come to refactory?
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        OpenAuthenticationStatus autheticationStatus = _openAuthorizer.Authorize(
                            new OpenIdAuthenticationParameters(_openIdRelyingPartyService.Response.ClaimedIdentifier, _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay));

                        if (autheticationStatus == OpenAuthenticationStatus.Authenticated)
                            return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
                        if (autheticationStatus == OpenAuthenticationStatus.ErrorAuthenticating) {
                            this.AddError(_openAuthorizer.Error.Key, T(_openAuthorizer.Error.Value));
                            return DefaultLogOnResult(returnUrl);
                        }
                        if (autheticationStatus == OpenAuthenticationStatus.RequiresRegistration) {
                            var registerModelBuilder = new RegisterModelBuilder(_openIdRelyingPartyService.Response);
                            var model = registerModelBuilder.Build();

                            TempData["registermodel"] = model;

                            return DefaultRegisterResult(returnUrl, model);
                        }
                        break;
                    case AuthenticationStatus.Canceled:
                        this.AddError("InvalidProvider", T("Canceled at provider"));
                        break;
                    case AuthenticationStatus.Failed:
                        this.AddError("UnknownError", T(_openIdRelyingPartyService.Response.Exception.Message));
                        break;
                }
            }

            return DefaultLogOnResult(returnUrl);
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn(string returnUrl) {
            CreateViewModel viewModel = new CreateViewModel();
            TryUpdateModel(viewModel);

            return BuildLogOnAuthenticationRedirect(viewModel);
        }

        private ActionResult BuildLogOnAuthenticationRedirect(CreateViewModel viewModel) {
            var identifier = new OpenIdIdentifier(viewModel.ExternalIdentifier);
            if (!identifier.IsValid) {
                this.AddError("ExternalIdentifier", T("Invalid Open ID identifier"));
                return DefaultLogOnResult(viewModel.ReturnUrl);
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);
                
                request.AddExtension(Claims.CreateClaimsRequest(_openAuthenticationService.GetSettings()));
                request.AddExtension(Claims.CreateFetchRequest(_openAuthenticationService.GetSettings()));

                return request.RedirectingResponse.AsActionResult();
            }
            catch (ProtocolException ex) {
                this.AddError("ProtocolException", T("Unable to authenticate: {0}", ex.Message));
            }
            return DefaultLogOnResult(viewModel.ReturnUrl);
        }

        private ActionResult DefaultLogOnResult(string returnUrl) {
            return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
        }

        private ActionResult DefaultRegisterResult(string returnUrl, RegisterModel model) {
            return RedirectToAction("Register", "Account", RouteValuesHelper.CreateRegisterRouteValueDictionary(returnUrl, model));
        }
    }
}