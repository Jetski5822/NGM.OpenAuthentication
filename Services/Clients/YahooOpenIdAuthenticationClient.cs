using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Services.Clients {
    public class YahooOpenIdAuthenticationClient : IExternalAuthenticationClient {
        public string ProviderName {
            get { return "Yahoo"; }
        }

        public IAuthenticationClient Build(ProviderConfigurationRecord providerConfigurationRecord) {
            return new YahooOpenIdClient();
        }
    }
}