using DotNetOpenAuth.AspNet;
using NGM.OpenAuthentication.Models;
using Orchard;

namespace NGM.OpenAuthentication.Services.Clients {
    public interface IExternalAuthenticationClient : IDependency {
        string ProviderName { get; }
        IAuthenticationClient Build(ProviderConfigurationRecord providerConfigurationRecord);
    }
}