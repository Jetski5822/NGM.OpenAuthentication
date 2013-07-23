using System;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OpenId;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Services.Clients {
    public class CustomOpenIdAuthenticationClient {
        private readonly string _providerName;

        public CustomOpenIdAuthenticationClient(string providerName) {
            _providerName = providerName;
        }

        public IAuthenticationClient Build(ProviderConfigurationRecord providerConfigurationRecord) {
            if (string.IsNullOrWhiteSpace(providerConfigurationRecord.ProviderIdentifier))
                throw new Exception("Client Identifier must be known if Provider is unknown.");

            return new OpenIdClient(_providerName, Identifier.Parse(providerConfigurationRecord.ProviderIdentifier));
        }
    }
}