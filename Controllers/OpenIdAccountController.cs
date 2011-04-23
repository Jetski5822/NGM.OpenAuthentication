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
        private readonly IOpenIdProviderAuthenticator _openIdProviderAuthenticator;
        private readonly IOrchardServices _orchardServices;

        public OpenIdAccountController(IOpenIdProviderAuthenticator openIdProviderAuthenticator,
            IOrchardServices orchardServices)
        {
            _openIdProviderAuthenticator = openIdProviderAuthenticator;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            if (!_openIdProviderAuthenticator.IsOpenIdCallback) {
                var viewModel = new CreateViewModel();
                TryUpdateModel(viewModel);
                _openIdProviderAuthenticator.EnternalIdentifier = viewModel.ExternalIdentifier;
            }
            var result = _openIdProviderAuthenticator.Authenticate(returnUrl);

            if (result.Status == OpenAuthenticationStatus.AssociateOnLogon) {
                return new RedirectResult(Url.LogOn(returnUrl));
            }
            else if (result.Status == OpenAuthenticationStatus.Authenticated) {
                _orchardServices.Notifier.Information(T("Account authenticated"));
            } 
            else if (result.Status != OpenAuthenticationStatus.RequresRedirect) {
                _orchardServices.Notifier.Error(T(result.Error.Value));
            }

            if (result.Result != null) return result.Result;

            return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
    }
}