using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using NGM.OpenAuthentication.Services;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Workflows.Models;

namespace NGM.OpenAuthentication.Activities {
    public class OAuthSignIn : TaskBase {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOrchardOpenAuthDataProvider _orchardOpenAuthDataProvider;
        private readonly IMembershipService _membershipService;

        public OAuthSignIn(IAuthenticationService authenticationService, IOrchardOpenAuthDataProvider orchardOpenAuthDataProvider, IMembershipService membershipService) {

            _authenticationService = authenticationService;
            _orchardOpenAuthDataProvider = orchardOpenAuthDataProvider;
            _membershipService = membershipService;
        }

        public const string ActivityName = "OAuthSignIn";
        
        public override string Name {
            get { return ActivityName; }
        }

        public override LocalizedString Category {
            get { return T("Authentication"); }
        }

        public override LocalizedString Description {
            get { return T("Signs in the local user associated with the OAuth account."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Failed");
            yield return T("Success");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var result = workflowContext.GetState<AuthenticationResultInfo>("OAuthResult");
            var userName = _orchardOpenAuthDataProvider.GetUserNameFromOpenAuth(result.Provider, result.ProviderUserId);

            if (string.IsNullOrWhiteSpace(userName))
                yield return T("Failed");

            _authenticationService.SignIn(_membershipService.GetUser(userName), true);
            yield return T("Success");
        }
    }
}