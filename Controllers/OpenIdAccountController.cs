using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Themes;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class OpenIdAccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IOpenAuthorizer _openAuthorizer;

        public OpenIdAccountController(IOpenIdRelyingPartyService openIdRelyingPartyService, 
            IOpenAuthenticationService openAuthenticationService, 
            IMembershipService membershipService, 
            IOpenAuthorizer openAuthorizer)
        {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _openAuthenticationService = openAuthenticationService;
            _membershipService = membershipService;
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
                            _openIdRelyingPartyService.Response.ClaimedIdentifier, _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay);

                        if (autheticationStatus == OpenAuthenticationStatus.Authenticated)
                            return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
                        if (autheticationStatus == OpenAuthenticationStatus.ErrorAuthenticating) {
                            AddError(_openAuthorizer.Error.Key, _openAuthorizer.Error.Value);
                            return DefaultLogOnResult(returnUrl);
                        }
                        if (autheticationStatus == OpenAuthenticationStatus.RequiresRegistration) {
                            var registerModelBuilder = new RegisterModelBuilder(_openIdRelyingPartyService.Response, _membershipService);
                            var model = registerModelBuilder.Build();

                            TempData["registermodel"] = model;

                            return DefaultRegisterResult(returnUrl, model);
                        }
                        break;
                    case AuthenticationStatus.Canceled:
                        AddError("InvalidProvider", "Canceled at provider");
                        break;
                    case AuthenticationStatus.Failed:
                        AddError("UnknownError", _openIdRelyingPartyService.Response.Exception.Message);
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
                AddError("ExternalIdentifier", "Invalid Open ID identifier");
                return DefaultLogOnResult(viewModel.ReturnUrl);
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);
                
                request.AddExtension(Claims.CreateClaimsRequest(_openAuthenticationService.GetSettings()));
                request.AddExtension(Claims.CreateFetchRequest(_openAuthenticationService.GetSettings()));

                return request.RedirectingResponse.AsActionResult();
            }
            catch (ProtocolException ex) {
                AddError("ProtocolException", string.Format("Unable to authenticate: {0}", ex.Message));
            }
            return DefaultLogOnResult(viewModel.ReturnUrl);
        }

        private void AddError(string key, string value) {
            var errorKey = string.Format("error-{0}", key);

            if (!TempData.ContainsKey(errorKey)) {
                TempData.Add(errorKey, value);
                ModelState.AddModelError(errorKey, value);
            } else {
                TempData[errorKey] = value;
            }
        }

        private ActionResult DefaultLogOnResult(string returnUrl) {
            return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
        }

        private ActionResult DefaultRegisterResult(string returnUrl, RegisterModel model) {
            return RedirectToAction("Register", "Account", new {
                area = "Orchard.Users",
                ReturnUrl = returnUrl,
                externalidentifier = model.ExternalIdentifier,
                externaldisplayidentifier = model.ExternalDisplayIdentifier
            });
        }
    }
}