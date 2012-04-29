using System;
using System.Security.Cryptography;
using NGM.OpenAuthentication.Core.Results;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Notify;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Core {
    public class Authenticator : IAuthenticator {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IOrchardServices _orchardServices;
        private readonly IStateBag _stateBag;

        public Authenticator(IAuthenticationService authenticationService,
                              IOpenAuthenticationService openAuthenticationService,
                              IMembershipService membershipService,
                              IOrchardServices orchardServices,
                              IStateBag stateBag) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
            _membershipService = membershipService;
            _orchardServices = orchardServices;
            _stateBag = stateBag;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public AuthenticationResult Authenticate(OpenAuthenticationParameters parameters) {
            var userFound = _openAuthenticationService.GetUser(parameters);

            var userLoggedIn = _authenticationService.GetAuthenticatedUser();

            if (AccountAlreadyExistsAndUserIsLoggedOn(userFound, userLoggedIn)) {
                if (AccountIsAssignedToLoggedOnAccount(userFound, userLoggedIn)) {
                    // The person is trying to log in as himself.. bit weird
                    _orchardServices.Notifier.Information(T("Account authenticated"));
                    return new AuthenticatedAuthenticationResult();
                }

                _orchardServices.Notifier.Warning(T("Account is already assigned"));
                return new AccountAlreadyAssignedAuthenticationResult();
            }
            if (AccountDoesNotExistAndUserIsNotLoggedOn(userFound, userLoggedIn)) {
                // If I am not logged in, and I noone has this identifier, then go to register page to get them to confirm details.
                var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();

                _stateBag.Parameters = parameters;

                if (AutoRegistrationIsEnabled(registrationSettings)) {
                    if (CanCreateAccount(parameters)) {
                        userFound = CreateUser(parameters);
                    }
                    else {
                        _orchardServices.Notifier.Error(T("User does not have enough details to auto create account"));
                        return new UserDoesNotHaveEnoughDetailsToAutoRegisterAuthenticationResult();
                    }
                } else if (RegistrationIsEnabled(registrationSettings)) {
                    _orchardServices.Notifier.Information(T("Your {0} account will be associated when you login", parameters.Provider.Name));
                    return new AuthenticationResult(Statuses.AssociateOnLogon);
                } else {
                    _orchardServices.Notifier.Warning(T("User does not exist on system"));
                    return new UserDoesNotExistAuthenticationResult();
                }
            }
            if (userFound == null) {
                _openAuthenticationService.AssociateExternalAccountWithUser(userLoggedIn, parameters);
            }

            _authenticationService.SignIn(userFound ?? userLoggedIn, false);

            _orchardServices.Notifier.Information(T("Account authenticated"));
            return new AuthenticatedAuthenticationResult();
        }

        private bool RegistrationIsEnabled(RegistrationSettingsPart registrationSettings) {
            return registrationSettings.UsersCanRegister && !_openAuthenticationService.GetSettings().Record.AutoRegisterEnabled;
        }

        private bool AutoRegistrationIsEnabled(RegistrationSettingsPart registrationSettings) {
            return registrationSettings.UsersCanRegister && _openAuthenticationService.GetSettings().Record.AutoRegisterEnabled;
        }

        private bool AccountDoesNotExistAndUserIsNotLoggedOn(IUser userFound, IUser userLoggedIn) {
            return userFound == null && userLoggedIn == null;
        }

        private bool AccountIsAssignedToLoggedOnAccount(IUser userFound, IUser userLoggedIn) {
            return userFound.Id.Equals(userLoggedIn.Id);
        }

        private bool AccountAlreadyExistsAndUserIsLoggedOn(IUser userFound, IUser userLoggedIn) {
            return userFound != null && userLoggedIn != null;
        }

        private bool CanCreateAccount(OpenAuthenticationParameters parameters) {
            return new RegistrationDetails(parameters).IsValid();
        }

        private IUser CreateUser(OpenAuthenticationParameters parameters) {
            var details = new RegistrationDetails(parameters);
            var randomArray = new byte[10];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            return _membershipService.CreateUser(new CreateUserParams(details.UserName, Convert.ToBase64String(randomArray), details.EmailAddress, null, null, true));
        }
    }
}