using System.Collections.Generic;

namespace NGM.OpenAuthentication.Security {
    public class OpenAuthCreateUserParams {
        public OpenAuthCreateUserParams(string userName, string providerName, string providerUserId,
                                        IDictionary<string, string> extraData) {
            UserName = userName;
            ProviderName = providerName;
            ProviderUserId = providerUserId;
            ExtraData = extraData;
        }

        public string UserName { get; private set; }
        public string ProviderName { get; private set; }
        public string ProviderUserId { get; private set; }
        public IDictionary<string, string> ExtraData { get; private set; }
    }
}