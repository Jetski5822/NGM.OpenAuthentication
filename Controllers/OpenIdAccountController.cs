using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class OpenIdAccountController : Controller {
        private readonly IOpenIdProviderAuthorizer _openIdProviderAuthorizer;
        private readonly IOrchardServices _orchardServices;

        public OpenIdAccountController(IOpenIdProviderAuthorizer openIdProviderAuthorizer,
            IOrchardServices orchardServices)
        {
            _openIdProviderAuthorizer = openIdProviderAuthorizer;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            if (!_openIdProviderAuthorizer.IsOpenIdCallback) {
                var viewModel = new CreateViewModel();
                TryUpdateModel(viewModel);
                _openIdProviderAuthorizer.EnternalIdentifier = viewModel.ExternalIdentifier;
            }
            var result = _openIdProviderAuthorizer.Authorize(returnUrl);

            if (result.AuthenticationStatus == OpenAuthenticationStatus.AssociateOnLogon) {
                return new RedirectResult(Url.LogOn(returnUrl));
            }
            else if (result.AuthenticationStatus == OpenAuthenticationStatus.Authenticated) {
                _orchardServices.Notifier.Information(T("Account authenticated"));
            } 
            else if (result.AuthenticationStatus != OpenAuthenticationStatus.RequresRedirect) {
                _orchardServices.Notifier.Error(T(result.Error.Value));
            }

            if (result.Result != null) return result.Result;

            return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
    }
}