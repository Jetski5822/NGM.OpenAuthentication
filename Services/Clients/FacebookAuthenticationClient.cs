using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Services.Clients {
    public class FacebookAuthenticationClient : IExternalAuthenticationClient {
        public string ProviderName {
            get { return "Facebook"; }
        }

        public IAuthenticationClient Build(ProviderConfigurationRecord providerConfigurationRecord) {
            return new FacebookClient(providerConfigurationRecord.ProviderIdKey, providerConfigurationRecord.ProviderSecret);
        }
    }
}