using System.Collections.Generic;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Core {
    public class OpenAuthorizer : IOpenAuthorizer {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IOrchardServices _orchardServices;

        public OpenAuthorizer(IAuthenticationService authenticationService,
                              IOpenAuthenticationService openAuthenticationService,
                              IOrchardServices orchardServices) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _orchardServices = orchardServices;
        }

        public KeyValuePair<string, string> Error { get; private set; }

        public OpenAuthenticationStatus Authorize(string externalIdentifier, string externalDisplayIdentifier) {
            var userFound = _openAuthenticationService.GetUser(externalIdentifier);

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

                if ((registrationSettings != null) &&
                    (registrationSettings.UsersCanRegister == false)) {
                    Error = new KeyValuePair<string, string>("AccessDenied", "User does not exist on system");
                    return OpenAuthenticationStatus.ErrorAuthenticating;
                }

                return OpenAuthenticationStatus.RequiresRegistration;
            }

            var user = userLoggedIn ?? userFound;

            _openAuthenticationService.AssociateExternalAccountWithUser(
                user,
                externalIdentifier,
                externalDisplayIdentifier);

            _authenticationService.SignIn(user, false);

            return OpenAuthenticationStatus.Authenticated;
        }
    }
}