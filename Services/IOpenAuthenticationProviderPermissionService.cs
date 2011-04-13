using NGM.OpenAuthentication.Core;
using Orchard;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationProviderPermissionService : IDependency {
        bool IsPermissionEnabled(string namedPermission, Provider provider);
    }
}