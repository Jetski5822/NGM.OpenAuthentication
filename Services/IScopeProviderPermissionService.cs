using System.Collections.Generic;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard;

namespace NGM.OpenAuthentication.Services {
    public interface IScopeProviderPermissionService : IDependency
    {
        bool IsPermissionEnabled(string scope, Provider provider);

        IEnumerable<ScopeProviderPermissionRecord> GetAll();
        IEnumerable<ScopeProviderPermissionRecord> Get(Provider provider);
        void Create(Provider provider, ScopePermission permissionProvider);
    }
}