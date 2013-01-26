using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Services.Clients {
    public class LinkedInAuthenticationClient : IExternalAuthenticationClient {
        public string ProviderName {
            get { return "LinkedIn"; }
        }

        public IAuthenticationClient Build(ProviderConfigurationRecord providerConfigurationRecord) {
            return new LinkedInClient(providerConfigurationRecord.ProviderIdKey, providerConfigurationRecord.ProviderSecret);
        }
    }
}