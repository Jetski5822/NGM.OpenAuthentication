using System.Collections.Generic;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard;

namespace NGM.OpenAuthentication.Services {
    public interface IScopeProviderPermissionService : IDependency
    {
        bool IsPermissionEnabled(string scope, AccessControlProvider provider);

        IEnumerable<ScopeProviderPermissionRecord> GetAll();
        IEnumerable<ScopeProviderPermissionRecord> Get(AccessControlProvider provider);
        void Create(AccessControlProvider provider, ScopePermission permissionProvider);
        void Update(Dictionary<int, bool> providerPermissions);
    }
}