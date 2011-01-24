using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI.Notify;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Controllers
{
    [Themed]
    public class AccountController : Controller {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOrchardServices _orchardServices;

        public AccountController(
            IOpenIdRelyingPartyService openIdRelyingPartyService, 
            IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            IOrchardServices orchardServices)
        {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult LogOn(string returnUrl) {
            if (_openIdRelyingPartyService.HasResponse) {
                // TODO : Not happy about this huge switch statement, consider a stratagy pattern possibly when I come to refactory?
                switch (_openIdRelyingPartyService.Response.Status) {
                    case AuthenticationStatus.Authenticated:
                        var userFound = _openAuthenticationService.GetUser(_openIdRelyingPartyService.Response.ClaimedIdentifier);

                        var userLoggedIn = _authenticationService.GetAuthenticatedUser();

                        if (userFound != null && userLoggedIn != null && userFound.Id.Equals(userLoggedIn.Id)) {
                            // The person is trying to log in as himself.. bit weird
                            return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
                        }
                        if (userFound != null && userLoggedIn != null && !userFound.Id.Equals(userLoggedIn.Id)) {
                            // The person is trying to log in as himself.. bit weird
                            AddError("IdentifierAssigned", "ClaimedIdentifier has already been assigned to another account");
                            break;
                        }
                        if (userFound == null && userLoggedIn == null) {
                            // If I am not logged in, and I noone has this identifier, then go to register page to get them to confirm details.

                            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();

                            if ((registrationSettings != null) &&
                                (registrationSettings.UsersCanRegister == false)) {
                                AddError("AccessDenied", "User registration is disabled");
                                break;
                            }
                            else {
                                var registerModelBuilder = new RegisterModelBuilder(_openIdRelyingPartyService.Response);
                                var model = registerModelBuilder.Build();
                                model.ReturnUrl = returnUrl;

                                TempData["RegisterModel"] = model;
                                return RedirectToAction("Register", "Account", new {area = "NGM.OpenAuthentication"});
                            }
                        }

                        var user = userLoggedIn ?? userFound;

                        _openAuthenticationService.AssociateOpenIdWithUser(
                            user,
                            _openIdRelyingPartyService.Response.ClaimedIdentifier,
                            _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay);

                        _authenticationService.SignIn(user, false);

                        return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
                    case AuthenticationStatus.Canceled:
                        AddError("InvalidProvider", "Canceled at provider");
                        break;
                    case AuthenticationStatus.Failed:
                        AddError("UnknownError", _openIdRelyingPartyService.Response.Exception.Message);
                        break;
                }
            }

            return DefaultLogOnResult();
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult _LogOn(string returnUrl) {
            LogOnViewModel viewModel = new LogOnViewModel();
            TryUpdateModel(viewModel);

            return BuildLogOnAuthenticationRedirect(viewModel);
        }

        public ActionResult Register(RegisterViewModel viewModel) {
            if (viewModel == null || viewModel.Model == null) {
                var model = TempData["RegisterModel"] as RegisterModel;
                if (model == null)
                    return DefaultLogOnResult();

                viewModel = new RegisterViewModel {Model = model};
            }

            return View("Register", viewModel);
        }

        [HttpPost, ActionName("Register")]
        public ActionResult _Register(RegisterViewModel viewModel) {
            if (ModelState.IsValid) {
                if (!_openAuthenticationService.IsAccountExists(viewModel.Model.ClaimedIdentifier)) {
                    var user = _openAuthenticationService.CreateUser(viewModel.Model);

                    // Sign In
                    _authenticationService.SignIn(user, false);

                    return Redirect(!string.IsNullOrEmpty(viewModel.Model.ReturnUrl) ? viewModel.Model.ReturnUrl : "~/");
                }
                AddError("IdentifierAssigned", "ClaimedIdentifier has already been assigned");
            }
            return View("Register", viewModel);
        }

        private ActionResult BuildLogOnAuthenticationRedirect(LogOnViewModel viewModel) {
            var identifier = new OpenIdIdentifier(viewModel.OpenIdIdentifier);
            if (!identifier.IsValid) {
                AddError("OpenIdIdentifier", "Invalid Open ID identifier");
                return DefaultLogOnResult();
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
            return DefaultLogOnResult();
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

        private ActionResult DefaultLogOnResult() {
            return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users" });
        }
    }
}