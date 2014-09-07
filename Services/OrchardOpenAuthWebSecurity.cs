using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
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
        private readonly IEncryptionService _encryptionService;

        public OrchardOpenAuthWebSecurity(IOpenAuthSecurityManagerWrapper openAuthSecurityManagerWrapper,
                                          IUserProviderServices userProviderServices,
                                          IOrchardOpenAuthClientProvider orchardOpenAuthClientProvider,
                                          IEncryptionService encryptionService) {
            _openAuthSecurityManagerWrapper = openAuthSecurityManagerWrapper;
            _userProviderServices = userProviderServices;
            _orchardOpenAuthClientProvider = orchardOpenAuthClientProvider;
            _encryptionService = encryptionService;
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

            var protectedBytes = ToByteArray(new SerializedProvider {ProviderName = providerName, ProviderUserId = providerUserId});
            return Convert.ToBase64String(_encryptionService.Encode(protectedBytes));
        }


        public bool TryDeserializeProviderUserId(string data, out string providerName, out string providerUserId) {
            Argument.ThrowIfNullOrEmpty(data, "data");

            var protectedBytes = _encryptionService.Decode(Convert.FromBase64String(data));
            var provider = (SerializedProvider) ToObject(protectedBytes);
            providerName = provider.ProviderName;
            providerUserId = provider.ProviderUserId;

            return true;
        }

        public OrchardAuthenticationClientData GetOAuthClientData(string providerName) {
            return _orchardOpenAuthClientProvider.GetClientData(providerName);
        }

        [Serializable]
        private struct SerializedProvider {
            public string ProviderName { get; set; }
            public string ProviderUserId { get; set; }
        }

        private byte[] ToByteArray(object source) {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream()) {
                formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }

        private object ToObject(byte[] source) {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(source)) {
                ms.Write(source, 0, source.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(ms);
            }
        }
    }
}