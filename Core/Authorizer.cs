using System;
using System.Collections.Generic;
using System.Web;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Core {
    public class Authorizer : IAuthorizer {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IOrchardServices _orchardServices;

        public Authorizer(IAuthenticationService authenticationService,
                              IOpenAuthenticationService openAuthenticationService,
                              IMembershipService membershipService,
                              IOrchardServices orchardServices) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _membershipService = membershipService;
            _orchardServices = orchardServices;
        }

        public KeyValuePair<string, string> Error { get; private set; }

        public OpenAuthenticationStatus Authorize(OpenAuthenticationParameters parameters) {
            var userFound = _openAuthenticationService.GetUser(parameters);

            var userLoggedIn = _authenticationService.GetAuthenticatedUser();

            if (userFound != null && userLoggedIn != null) {
                if (userFound.Id.Equals(userLoggedIn.Id)) {
                    // The person is trying to log in as himself.. bit weird
                    return OpenAuthenticationStatus.Authenticated;
                }
                Error = new KeyValuePair<string, string>("AccountAssigned", "Account is already assigned");
                return OpenAuthenticationStatus.ErrorAuthenticating;
            }
            if (userFound == null && userLoggedIn == null) {
                // If I am not logged in, and I noone has this identifier, then go to register page to get them to confirm details.
                var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();

                StoreParametersForRoundTrip(parameters);

                if (registrationSettings.UsersCanRegister == true && _openAuthenticationService.GetSettings().Record.AutoRegisterEnabled == true) {
                    if (CanCreateAccount(parameters)) {
                        userFound = CreateUser(parameters);
                    }
                    else
                    {
                        Error = new KeyValuePair<string, string>("AccessDenied", "User does not have enough details to auto create account");
                        return OpenAuthenticationStatus.RequiresRegistration;
                    }
                } else if (registrationSettings.UsersCanRegister == true && _openAuthenticationService.GetSettings().Record.AutoRegisterEnabled == false) {
                    return OpenAuthenticationStatus.RequiresRegistration;
                } else {
                    Error = new KeyValuePair<string, string>("AccessDenied", "User does not exist on system");
                    return OpenAuthenticationStatus.ErrorAuthenticating;
                }
            }
            if (userFound == null) {
                _openAuthenticationService.AssociateExternalAccountWithUser(userLoggedIn, parameters);
            }

            _authenticationService.SignIn(userFound ?? userLoggedIn, false);

            return OpenAuthenticationStatus.Authenticated;
        }

        public static OpenAuthenticationParameters RetrieveParametersFromRoundTrip(bool removeOnRetrieval) {
            var parameters = HttpContext.Current.Session["parameters"];
            if (parameters != null && removeOnRetrieval)
                HttpContext.Current.Session.Remove("parameters");

            return parameters as OpenAuthenticationParameters;
        }

        private void StoreParametersForRoundTrip(OpenAuthenticationParameters parameters) {
            _orchardServices.WorkContext.HttpContext.Session["parameters"] = parameters;
        }

        private bool CanCreateAccount(OpenAuthenticationParameters parameters) {
            var registerModel = new RegisterModel(parameters);
            RegisterModelHelper.PopulateModel(parameters.UserClaims, registerModel);

            return !string.IsNullOrEmpty(registerModel.UserName) && (!string.IsNullOrEmpty(registerModel.Email));
        }

        private IUser CreateUser(OpenAuthenticationParameters parameters) {
            var registerModel = new RegisterModel(parameters);
            RegisterModelHelper.PopulateModel(parameters.UserClaims, registerModel);

            return _membershipService.CreateUser(new CreateUserParams(registerModel.UserName, new Byte[10].ToString(), registerModel.Email, null, null, true));
        }
    }
}