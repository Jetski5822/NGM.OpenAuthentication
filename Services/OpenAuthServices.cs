using System.Collections.Generic;
using NGM.OpenAuthentication.Events;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Security;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthMembershipServices : IDependency {
        bool CanRegister();
        IUser CreateUser(OpenAuthCreateUserParams createUserParams);
    }

    public class OpenAuthMembershipServices : IOpenAuthMembershipServices {
        private readonly IOrchardServices _orchardServices;
        private readonly IMembershipService _membershipService;
        private readonly IUsernameService _usernameService;
        private readonly IPasswordGeneratorService _passwordGeneratorService;
        private readonly IEnumerable<IOpenAuthUserEventHandler> _openAuthUserEventHandlers;

        public OpenAuthMembershipServices(IOrchardServices orchardServices,
            IMembershipService membershipService,
            IUsernameService usernameService,
            IPasswordGeneratorService passwordGeneratorService,
            IEnumerable<IOpenAuthUserEventHandler> openAuthUserEventHandlers) {
            _orchardServices = orchardServices;
            _membershipService = membershipService;
            _usernameService = usernameService;
            _passwordGeneratorService = passwordGeneratorService;
            _openAuthUserEventHandlers = openAuthUserEventHandlers;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public bool CanRegister() {
            var openAuthenticationSettings = _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>();
            var orchardUsersSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();

            return orchardUsersSettings.UsersCanRegister && openAuthenticationSettings.AutoRegistrationEnabled;
        }

        public IUser CreateUser(OpenAuthCreateUserParams createUserParams) {
            string emailAddress = string.Empty;
            if (createUserParams.UserName.IsEmailAddress()) {
                emailAddress = createUserParams.UserName;
            }

            var creatingContext = new CreatingOpenAuthUserContext(createUserParams.UserName, emailAddress, createUserParams.ProviderName, createUserParams.ProviderUserId, createUserParams.ExtraData);

            _openAuthUserEventHandlers.Invoke(o => o.Creating(creatingContext), Logger);

            var createdUser = _membershipService.CreateUser(new CreateUserParams(
                _usernameService.Calculate(createUserParams.UserName),
                _passwordGeneratorService.Generate(),
                creatingContext.EmailAddress,
                @T("Auto Registered User").Text,
                _passwordGeneratorService.Generate() /* Noone can guess this */,
                true
                ));

            var createdContext = new CreatedOpenAuthUserContext(createdUser, createUserParams.ProviderName, createUserParams.ProviderUserId, createUserParams.ExtraData);
            _openAuthUserEventHandlers.Invoke(o => o.Created(createdContext), Logger);

            return createdUser;
        }
    }
}