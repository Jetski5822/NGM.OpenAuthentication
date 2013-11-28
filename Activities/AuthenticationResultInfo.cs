using System;
using System.Collections.Generic;
using System.Linq;
using DotNetOpenAuth.AspNet;

namespace NGM.OpenAuthentication.Activities {
    [Serializable]
    public class AuthenticationResultInfo {
        public string ErrorMessage { get; set; }
        public Dictionary<string, string> ExtraData { get; set; }
        public bool IsSuccessful { get; set; }
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public string UserName { get; set; }

        public static AuthenticationResultInfo FromAuthenticationResult(AuthenticationResult value) {
            return new AuthenticationResultInfo {
                ErrorMessage = value.Error != null ? value.Error.Message : null,
                ExtraData = value.ExtraData != null ? value.ExtraData.ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>(),
                IsSuccessful = value.IsSuccessful,
                Provider = value.Provider,
                ProviderUserId = value.ProviderUserId,
                UserName = value.UserName
            };
        }
    }
}