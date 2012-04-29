using System.Collections.Generic;
using System.Linq;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Security;
using Orchard.Users.Events;

namespace NGM.OpenAuthentication.Handlers {
    public class AccountAssociationUserEventHandler : IUserEventHandler {
        private readonly IOrchardServices _orchardServices;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IEnumerable<IAccessControlProvider> _accessControlProviders;
        private readonly IStateBag _stateBag;

        public AccountAssociationUserEventHandler(
            IOrchardServices orchardServices,
            IOpenAuthenticationService openAuthenticationService,
            IEnumerable<IAccessControlProvider> accessControlProviders,
            IStateBag stateBag) {
            _orchardServices = orchardServices;
            _openAuthenticationService = openAuthenticationService;
            _accessControlProviders = accessControlProviders;
            _stateBag = stateBag;
        }

        public void Creating(UserContext context) {
        }

        public void Created(UserContext context) {
        }

        public void LoggedIn(IUser user) {
            if (HasQueryParamsLocator()) {
                TryAssociateAccount(user, GetQueryStringParameters());
            }
            else {
                var parameters = _stateBag.Parameters;
                if (parameters != null) {
                    _stateBag.Clear();
                    TryAssociateAccount(user, parameters);
                }
            }
        }

        public void LoggedOut(IUser user) {
            _stateBag.Clear();
        }

        public void AccessDenied(IUser user) {
        }

        public void ChangedPassword(IUser user) {
        }

        public void SentChallengeEmail(IUser user) {
        }

        public void ConfirmedEmail(IUser user) {
        }

        public void Approved(IUser user) {
        }

        // TODO Move to more appropriate location
        private OpenAuthenticationParameters GetQueryStringParameters() {
            var externalIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externalidentifier"];
            var externalDisplayIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externaldisplayidentifier"];
            var oAuthToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthtoken"];
            var oAuthAccessToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthaccesstoken"];
            var provider = _orchardServices.WorkContext.HttpContext.Request.Params["provider"];

            return new HashedOpenAuthenticationParameters(_accessControlProviders.First(x => x.Hash == provider)) {
                ExternalIdentifier = externalIdentifier,
                ExternalDisplayIdentifier = externalDisplayIdentifier,
                OAuthToken = oAuthToken,
                OAuthAccessToken = oAuthAccessToken
            };
        }

        private bool HasQueryParamsLocator() {
            return !string.IsNullOrEmpty(_orchardServices.WorkContext.HttpContext.Request.Params["provider"]);
        }

        private void TryAssociateAccount(IUser user, OpenAuthenticationParameters parameters) {
            if (parameters != null && !_openAuthenticationService.AccountExists(parameters)) {
                _openAuthenticationService.AssociateExternalAccountWithUser(user, parameters);
            }
        }
    }
}