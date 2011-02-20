using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class OpenIdAccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOpenAuthorizer _openAuthorizer;
        private readonly IOrchardServices _orchardServices;

        public OpenIdAccountController(IOpenIdRelyingPartyService openIdRelyingPartyService, IOpenAuthenticationService openAuthenticationService, IOpenAuthorizer openAuthorizer, IOrchardServices orchardServices)
        {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _openAuthenticationService = openAuthenticationService;
            _openAuthorizer = openAuthorizer;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                // TODO : Not happy about this huge switch statement, consider a stratagy pattern possibly when I come to refactory?
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var parameters = new OpenIdAuthenticationParameters(_openIdRelyingPartyService.Response.ClaimedIdentifier, _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay);
                        var autheticationStatus = _openAuthorizer.Authorize(parameters);

                        if (autheticationStatus == OpenAuthenticationStatus.Authenticated) {
                            _orchardServices.Notifier.Information(T("Account succesfully associated to logged in account"));
                            return this.RedirectLocal(returnUrl, "~/");
                        }
                        else if (autheticationStatus == OpenAuthenticationStatus.ErrorAuthenticating) {
                            _orchardServices.Notifier.Error(T(_openAuthorizer.Error.Value));
                        }
                        else if (autheticationStatus == OpenAuthenticationStatus.RequiresRegistration) {
                            var registerModelBuilder = new RegisterModelBuilder(_openIdRelyingPartyService.Response);
                            var model = registerModelBuilder.Build();

                            TempData["registermodel"] = model;

                            return new RedirectResult(Url.Register(returnUrl, model));
                        }
                        break;
                    case AuthenticationStatus.Canceled:
                        _orchardServices.Notifier.Error(T("Canceled at provider"));
                        break;
                    case AuthenticationStatus.Failed:
                        _orchardServices.Notifier.Error(T(_openIdRelyingPartyService.Response.Exception.Message));
                        break;
                }
            }

            return new RedirectResult(Url.LogOn(returnUrl));
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn(string returnUrl) {
            CreateViewModel viewModel = new CreateViewModel();
            TryUpdateModel(viewModel);

            var identifier = new OpenIdIdentifier(viewModel.ExternalIdentifier);
            if (!identifier.IsValid) {
                _orchardServices.Notifier.Error(T("Invalid Open ID identifier"));
                return new RedirectResult(Url.LogOn(viewModel.ReturnUrl));
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);
                
                request.AddExtension(Claims.CreateClaimsRequest(_openAuthenticationService.GetSettings()));
                request.AddExtension(Claims.CreateFetchRequest(_openAuthenticationService.GetSettings()));

                return request.RedirectingResponse.AsActionResult();
            }
            catch (ProtocolException ex) {
                _orchardServices.Notifier.Error(T("Unable to authenticate: {0}", ex.Message));
            }
            return new RedirectResult(Url.LogOn(viewModel.ReturnUrl));
        }
    }
}