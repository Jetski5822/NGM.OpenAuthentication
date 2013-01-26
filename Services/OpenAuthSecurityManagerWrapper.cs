using DotNetOpenAuth.AspNet;
using Orchard;
using Orchard.Mvc;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthSecurityManagerWrapper : IDependency {
        bool Login(string providerUserId, bool createPersistentCookie);
        AuthenticationResult VerifyAuthentication(string returnUrl);
        void RequestAuthentication(string providerName, string returnUrl);
    }

    public class OpenAuthSecurityManagerWrapper : IOpenAuthSecurityManagerWrapper {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrchardOpenAuthClientProvider _orchardOpenAuthClientProvider;
        private readonly IOrchardOpenAuthDataProvider _orchardOpenAuthDataProvider;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMembershipService _membershipService;

        public OpenAuthSecurityManagerWrapper(IHttpContextAccessor httpContextAccessor, 
                                              IOrchardOpenAuthClientProvider orchardOpenAuthClientProvider,
                                              IOrchardOpenAuthDataProvider orchardOpenAuthDataProvider,
                                              IAuthenticationService authenticationService,
                                              IMembershipService membershipService) {
            _httpContextAccessor = httpContextAccessor;
            _orchardOpenAuthClientProvider = orchardOpenAuthClientProvider;
            _orchardOpenAuthDataProvider = orchardOpenAuthDataProvider;
            _authenticationService = authenticationService;
            _membershipService = membershipService;
        }

        private string ProviderName {
            get { return OpenAuthSecurityManager.GetProviderName(_httpContextAccessor.Current()); }
        }

        public bool Login(string providerUserId, bool createPersistentCookie) {
            string userName = _orchardOpenAuthDataProvider.GetUserNameFromOpenAuth(ProviderName, providerUserId);

            if (string.IsNullOrWhiteSpace(userName))
                return false;

            _authenticationService.SignIn(_membershipService.GetUser(userName), createPersistentCookie);

            return true;
        }

        public AuthenticationResult VerifyAuthentication(string returnUrl) {
            return SecurityManager(ProviderName).VerifyAuthentication(returnUrl);
        }

        public void RequestAuthentication(string providerName, string returnUrl) {
            SecurityManager(providerName).RequestAuthentication(returnUrl);
        }

        private OpenAuthSecurityManager SecurityManager(string providerName) {
            return new OpenAuthSecurityManager(_httpContextAccessor.Current(), _orchardOpenAuthClientProvider.GetClient(providerName), _orchardOpenAuthDataProvider); 
        }
    }
}