using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.Security;
using Orchard.Validation;

namespace NGM.OpenAuthentication.Services {
    public interface IOrchardOpenAuthWebSecurity : IDependency {
        AuthenticationResult VerifyAuthentication(string returnUrl);
        bool Login(string providerName, string providerUserId, bool createPersistantCookie = false);
        void CreateOrUpdateAccount(string providerName, string providerUserId, IUser user);
        string SerializeProviderUserId(string providerName, string providerUserId);
        OrchardAuthenticationClientData GetOAuthClientData(string providerName);
        bool TryDeserializeProviderUserId(string data, out string providerName, out string providerUserId);
    }

    public class OrchardOpenAuthWebSecurity : IOrchardOpenAuthWebSecurity {
        private readonly IOpenAuthSecurityManagerWrapper _openAuthSecurityManagerWrapper;
        private readonly IUserProviderServices _userProviderServices;
        private readonly IOrchardOpenAuthClientProvider _orchardOpenAuthClientProvider;

        public OrchardOpenAuthWebSecurity(IOpenAuthSecurityManagerWrapper openAuthSecurityManagerWrapper,
                                          IUserProviderServices userProviderServices,
                                          IOrchardOpenAuthClientProvider orchardOpenAuthClientProvider) {
            _openAuthSecurityManagerWrapper = openAuthSecurityManagerWrapper;
            _userProviderServices = userProviderServices;
            _orchardOpenAuthClientProvider = orchardOpenAuthClientProvider;
        }

        public AuthenticationResult VerifyAuthentication(string returnUrl) {
            return _openAuthSecurityManagerWrapper.VerifyAuthentication(returnUrl);
        }

        public bool Login(string providerName, string providerUserId, bool createPersistantCookie = false) {
            return _openAuthSecurityManagerWrapper.Login(providerUserId, createPersistantCookie);
        }

        public void CreateOrUpdateAccount(string providerName, string providerUserId, IUser user) {
            if (user == null)
                throw new MembershipCreateUserException(MembershipCreateStatus.ProviderError);

            var record = _userProviderServices.Get(providerName, providerUserId);

            if (record == null) {
                _userProviderServices.Create(providerName, providerUserId, user);
            }
            else {
                _userProviderServices.Update(providerName, providerUserId, user);
            }
        }

        public string SerializeProviderUserId(string providerName, string providerUserId) {
            Argument.ThrowIfNullOrEmpty(providerName, "providerName");
            Argument.ThrowIfNullOrEmpty(providerUserId, "providerUserId");

            return ProviderUserIdSerializationHelper.ProtectData(providerName, providerUserId);
        }


        public bool TryDeserializeProviderUserId(string data, out string providerName, out string providerUserId) {
            Argument.ThrowIfNullOrEmpty(data, "data");

            return ProviderUserIdSerializationHelper.UnprotectData(data, out providerName, out providerUserId);
        }

        public OrchardAuthenticationClientData GetOAuthClientData(string providerName) {
            return _orchardOpenAuthClientProvider.GetClientData(providerName);
        }
    }
}