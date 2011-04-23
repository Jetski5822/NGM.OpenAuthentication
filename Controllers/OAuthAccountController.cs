using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OAuth;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Controllers {
    [Themed]
    public class OAuthAccountController : Controller {
        private readonly IEnumerable<IOAuthProviderAuthenticator> _oAuthWrappers;
        private readonly IOrchardServices _orchardServices;

        public OAuthAccountController(IEnumerable<IOAuthProviderAuthenticator> oAuthWrappers, IOrchardServices orchardServices) {
            _oAuthWrappers = oAuthWrappers;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl, string knownProvider) {
            var viewModel = new CreateViewModel();
            TryUpdateModel(viewModel);

            var wrapper = _oAuthWrappers
                .Where(o => o.Provider.ToString().Equals(GetKnownProvider(viewModel, knownProvider), StringComparison.InvariantCultureIgnoreCase) 
                    && o.IsConsumerConfigured).FirstOrDefault();

            if (wrapper != null) {
                var result = wrapper.Authenticate(returnUrl);

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
            }

            return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }

        public string GetKnownProvider(CreateViewModel viewModel, string tempKnownProvider) {
            return string.IsNullOrEmpty(viewModel.KnownProvider) && !string.IsNullOrEmpty(tempKnownProvider) ? tempKnownProvider : viewModel.KnownProvider;
        }
    }
}