using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Orchard.Validation;

namespace NGM.OpenAuthentication.Models {
    public class OrchardAuthenticationClientData {
        public OrchardAuthenticationClientData(
            IAuthenticationClient authenticationClient,
            string displayName,
            IDictionary<string, object> extraData) {

            Argument.ThrowIfNull(authenticationClient, "authenticationClient");

            AuthenticationClient = authenticationClient;
            DisplayName = displayName;
            ExtraData = extraData;
        }

        public IAuthenticationClient AuthenticationClient { get; private set; }
        public string ProviderName { get { return AuthenticationClient.ProviderName; } }
        public string DisplayName { get; private set; }
        public IDictionary<string, object> ExtraData { get; private set; }
    }
}