using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Services.Clients {
    public class GoogleOpenIdAuthenticationClient : IExternalAuthenticationClient {
        public string ProviderName {
            get { return "Google"; }
        }

        public IAuthenticationClient Build(ProviderConfigurationRecord providerConfigurationRecord) {
            return new GoogleOpenIdClient();
        }
    }
}