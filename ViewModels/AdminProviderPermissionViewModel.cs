using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class AdminProviderPermissionViewModel {
        public IDictionary<string, IEnumerable<ScopeProviderPermissionRecord>> ProviderPermissions { get; set; }
    }
}