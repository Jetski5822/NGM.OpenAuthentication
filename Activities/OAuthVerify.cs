using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Services;
using Orchard.Localization;
using Orchard.Workflows.Models;

namespace NGM.OpenAuthentication.Activities {
    public class OAuthVerify : EventBase {
        public const string ActivityName = "OAuthVerify";
        private readonly UrlHelper _urlHelper;
        private readonly IOrchardOpenAuthWebSecurity _orchardOpenAuthWebSecurity;
        private readonly IOrchardOpenAuthDataProvider _orchardOpenAuthDataProvider;

        public OAuthVerify(UrlHelper urlHelper, IOrchardOpenAuthWebSecurity orchardOpenAuthWebSecurity, IOrchardOpenAuthDataProvider orchardOpenAuthDataProvider) {
            _urlHelper = urlHelper;
            _orchardOpenAuthWebSecurity = orchardOpenAuthWebSecurity;
            _orchardOpenAuthDataProvider = orchardOpenAuthDataProvider;
        }

        public override string Name {
            get { return ActivityName; }
        }

        public override LocalizedString Category {
            get { return T("Authentication"); }
        }

        public override LocalizedString Description {
            get { return T("Invoked when an OAuth response is received."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T("Failed");
            yield return T("AssociationFound");
            yield return T("AssociationNotFound");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var result = AuthenticationResultInfo.FromAuthenticationResult(_orchardOpenAuthWebSecurity.VerifyAuthentication(_urlHelper.OpenAuthLogOn(null)));

            if (!result.IsSuccessful) {
                yield return T("Failed");
            }

            var userName = _orchardOpenAuthDataProvider.GetUserNameFromOpenAuth(result.Provider, result.ProviderUserId);
            workflowContext.SetState("OAuthResult", result);
            yield return !String.IsNullOrWhiteSpace(userName) ? T("AssociationFound") : T("AssociationNotFound");
        }
    }
}