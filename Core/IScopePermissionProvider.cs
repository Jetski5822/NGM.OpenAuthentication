using System.Collections.Generic;
using Orchard;
using Orchard.Environment.Extensions.Models;

namespace NGM.OpenAuthentication.Core
{
    public interface IScopePermissionProvider : IDependency
    {
        Feature Feature { get; }
        AccessControlProvider Provider { get; }
        IEnumerable<ScopePermission> GetPermissions();
    }
}