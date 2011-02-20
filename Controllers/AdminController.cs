using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.Core.Contents.Controllers;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Controllers {
    [Admin]
    public class AdminController : Controller {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IOrchardServices _orchardServices;
        private readonly IOpenAuthorizer _openAuthorizer;

        public AdminController(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            IOpenIdRelyingPartyService openIdRelyingPartyService,
            IOrchardServices orchardServices,
            IOpenAuthorizer openAuthorizer) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _orchardServices = orchardServices;
            _openAuthorizer = openAuthorizer;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult Index() {
            var user = _authenticationService.GetAuthenticatedUser();
            var entries =
                _openAuthenticationService
                    .GetExternalIdentifiersFor(user)
                    .List()
                    .ToList()
                    .Select(account => CreateAccountEntry(account.Record));

            var viewModel = new AdminIndexViewModel {
                Accounts = entries.ToList(),
                Options = new AdminIndexOptions()
            };

            return View("Index", viewModel);
        }

        [HttpPost]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult Index(FormCollection input) {
            var viewModel = new AdminIndexViewModel { Accounts = new List<AccountEntry>() };
            UpdateModel(viewModel, input);

            var checkedEntries = viewModel.Accounts.Where(c => c.IsChecked);
            switch (viewModel.Options.BulkAction) {
                case AdminBulkAction.None:
                    break;
                case AdminBulkAction.Delete:
                    foreach (var entry in checkedEntries) {
                        _openAuthenticationService.RemoveAssociation(new OpenIdAuthenticationParameters(entry.Account.ExternalIdentifier) );
                    }
                    break;
            }
            
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Create(string returnUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                // TODO : Not happy about this huge switch statement, consider a stratagy pattern possibly when I come to refactory?
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var parameters = new OpenIdAuthenticationParameters(_openIdRelyingPartyService.Response.ClaimedIdentifier, _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay);
                        var status = _openAuthorizer.Authorize(parameters);

                        if (status == OpenAuthenticationStatus.Authenticated) {
                            _orchardServices.Notifier.Information(T("Account succesfully associated to logged in account"));
                            return this.RedirectLocal(returnUrl, "~/");
                        }

                        _orchardServices.Notifier.Error(T(_openAuthorizer.Error.Value));
                        break;
                    case AuthenticationStatus.Canceled:
                        _orchardServices.Notifier.Error(T("Canceled at provider"));
                        break;
                    case AuthenticationStatus.Failed:
                        _orchardServices.Notifier.Error(T(_openIdRelyingPartyService.Response.Exception.Message));
                        break;
                }
            }

            return View("Create");
        }

        [HttpPost, ActionName("CreateOpenId")]
        public ActionResult CreateOpenId(FormCollection formCollection) {
            var viewModel = new CreateViewModel();
            TryUpdateModel(viewModel, formCollection);

            var identifier = new OpenIdIdentifier(viewModel.ExternalIdentifier);
            if (!identifier.IsValid) {
                _orchardServices.Notifier.Error(T("Invalid Open ID identifier"));
            } else {
                try {
                    var request = _openIdRelyingPartyService.CreateRequest(identifier);

                    request.AddExtension(Claims.CreateClaimsRequest(_openAuthenticationService.GetSettings()));
                    request.AddExtension(Claims.CreateFetchRequest(_openAuthenticationService.GetSettings()));

                    return request.RedirectingResponse.AsActionResult();
                }
                catch (ProtocolException ex) {
                    _orchardServices.Notifier.Error(T("Unable to authenticate: {0}", ex.Message));
                }
            }
            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(string externalIdentifier, string returnUrl, int? hashedProvider) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage OpenID")))
                return new HttpUnauthorizedResult();

            var provider = new HashedOpenAuthenticationParameters(hashedProvider.GetValueOrDefault()) { ExternalIdentifier = externalIdentifier };

            try {
                _openAuthenticationService.RemoveAssociation(provider);
                
                _orchardServices.Notifier.Information(T("Account was successfully deleted."));
            } catch (Exception exception) {
                _orchardServices.Notifier.Error(T("Editing Account failed: {0}", exception.Message));
            }
            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        private AccountEntry CreateAccountEntry(OpenAuthenticationPartRecord openAuthenticationPart) {
            return new AccountEntry {
                Account = openAuthenticationPart
            };
        }
    }
}