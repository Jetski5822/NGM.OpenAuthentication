using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class OpenIdAccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOpenIdProviderAuthorizer _openIdProviderAuthorizer;
        private readonly IOrchardServices _orchardServices;

        public OpenIdAccountController(IOpenIdRelyingPartyService openIdRelyingPartyService, 
            IOpenAuthenticationService openAuthenticationService, 
            IOpenIdProviderAuthorizer openIdProviderAuthorizer,
            IOrchardServices orchardServices)
        {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _openAuthenticationService = openAuthenticationService;
            _openIdProviderAuthorizer = openIdProviderAuthorizer;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            if (_openIdProviderAuthorizer.IsOpenIdCallback) {
                var result = _openIdProviderAuthorizer.Authorize(returnUrl);

                if (result.AuthenticationStatus == OpenAuthenticationStatus.ErrorAuthenticating) {
                    _orchardServices.Notifier.Error(T(result.Error.Value));
                }
                
                if (result.AuthenticationStatus == OpenAuthenticationStatus.RequiresRegistration) {
                    TempData["registermodel"] = result.RegisterModel;
                    return new RedirectResult(Url.Register(returnUrl, result.RegisterModel));
                }
                
                if (result.AuthenticationStatus == OpenAuthenticationStatus.Authenticated) {
                    _orchardServices.Notifier.Information(T("Account succesfully associated to logged in account"));
                    return new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
                }

                if (result.Result != null) return result.Result;
            }
            return new RedirectResult(Url.Referer(this.Request));
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn(string returnUrl) {
            CreateViewModel viewModel = new CreateViewModel();
            TryUpdateModel(viewModel);

            var identifier = new OpenIdIdentifier(viewModel.ExternalIdentifier);
            if (!identifier.IsValid) {
                _orchardServices.Notifier.Error(T("Invalid Open ID identifier"));
                return new RedirectResult(Url.Referer(this.Request));
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
            
            return new RedirectResult(Url.Referer(this.Request));
        }
    }
}