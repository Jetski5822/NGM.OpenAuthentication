using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Services.Clients {
    public class TwitterAuthenticationClient : IExternalAuthenticationClient {
        public string ProviderName {
            get { return "Twitter"; }
        }

        public IAuthenticationClient Build(ProviderConfigurationRecord providerConfigurationRecord) {
            return new TwitterClient(providerConfigurationRecord.ProviderIdKey, providerConfigurationRecord.ProviderSecret);
        }
    }
}