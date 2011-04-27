using System.Collections.Generic;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationProviderPermissionService : IDependency {
        bool IsPermissionEnabled(string scope, Provider provider);

        IEnumerable<OpenAuthenticationPermissionSettingsPart> GetAll();
        IEnumerable<OpenAuthenticationPermissionSettingsPart> Get(Provider provider);
    }
}