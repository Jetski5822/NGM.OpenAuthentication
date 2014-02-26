using System.Security.Policy;
using System.Web;
using NGM.OpenAuthentication.Services;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Users.Events;

namespace NGM.OpenAuthentication.Events {
    public class OpenAuthUserEventHandler : IUserEventHandler {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrchardOpenAuthWebSecurity _orchardOpenAuthWebSecurity;

        public OpenAuthUserEventHandler(IHttpContextAccessor httpContextAccessor, 
            IOrchardOpenAuthWebSecurity orchardOpenAuthWebSecurity) {
            _httpContextAccessor = httpContextAccessor;
            _orchardOpenAuthWebSecurity = orchardOpenAuthWebSecurity;
        }

        public void Creating(UserContext context) {
        }

        public void Created(UserContext context) {
            CreateOrUpdateOpenAuthUser(context.User);
        }

        public void LoggedIn(IUser user) {
            CreateOrUpdateOpenAuthUser(user);
        }

        private void CreateOrUpdateOpenAuthUser(IUser user) {
            var current = _httpContextAccessor.Current();
            if (current == null)
                return;

            var request = current.Request;

            if (request == null)
                return;

            var userName = request.QueryString["UserName"];
            var externalLoginData = request.QueryString["ExternalLoginData"];

            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(externalLoginData))
                return;

            string providerName;
            string providerUserId;

            if (
                !_orchardOpenAuthWebSecurity.TryDeserializeProviderUserId(HttpUtility.UrlDecode(externalLoginData), out providerName,
                                                                          out providerUserId))
                return;

            _orchardOpenAuthWebSecurity.CreateOrUpdateAccount(providerName, providerUserId, user);
        }

        public void LoggedOut(IUser user) {
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
    }
}