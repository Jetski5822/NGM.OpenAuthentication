using System.Collections.Generic;
using NGM.OpenAuthentication.Security;
using NGM.OpenAuthentication.Services;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Workflows.Models;

namespace NGM.OpenAuthentication.Activities {
    public class OAuthCreateOrAssociateUser : TaskBase {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOrchardOpenAuthWebSecurity _orchardOpenAuthWebSecurity;
        private readonly IOpenAuthMembershipServices _openAuthMembershipServices;

        public OAuthCreateOrAssociateUser(
            IAuthenticationService authenticationService, 
            IOrchardOpenAuthWebSecurity orchardOpenAuthWebSecurity, 
            IOpenAuthMembershipServices openAuthMembershipServices) {

            _authenticationService = authenticationService;
            _orchardOpenAuthWebSecurity = orchardOpenAuthWebSecurity;
            _openAuthMembershipServices = openAuthMembershipServices;
        }

        public const string ActivityName = "OAuthCreateOrAssociateUser";
        
        public override string Name {
            get { return ActivityName; }
        }

        public override LocalizedString Category {
            get { return T("Authentication"); }
        }

        public override LocalizedString Description {
            get { return T("Creates a new or associates the current user with the OAuth user token."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("CurrentUserAssociated");
            yield return T("NewUserAssociated");
            yield return T("AssociationPending");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var result = workflowContext.GetState<AuthenticationResultInfo>("OAuthResult");
            var authenticatedUser = _authenticationService.GetAuthenticatedUser();

            if (authenticatedUser != null) {
                _orchardOpenAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, authenticatedUser);
                yield return T("CurrentUserAssociated");
            }
            else if (_openAuthMembershipServices.CanRegister()) {
                var newUser = _openAuthMembershipServices.CreateUser(new OpenAuthCreateUserParams(result.UserName, result.Provider, result.ProviderUserId, result.ExtraData));
                _orchardOpenAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, newUser);
                workflowContext.SetState("CreatedUser", newUser.UserName);
                yield return T("NewUserAssociated");
            }
            else {
                yield return T("AssociationPending");                
            }
        }
    }
}