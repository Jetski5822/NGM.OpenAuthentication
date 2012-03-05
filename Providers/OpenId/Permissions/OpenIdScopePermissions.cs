using System.Collections.Generic;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Providers.OpenId.Services;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;

namespace NGM.OpenAuthentication.Providers.OpenId.Permissions {
    [OrchardFeature("OpenId")]
    public class OpenIdScopePermissions : IScopePermissionProvider {
        public virtual Feature Feature { get; set; }

        public AccessControlProvider Provider {
            get { return new OpenIdAccessControlProvider(); }
        }

        public IEnumerable<ScopePermission> GetPermissions() {
            return new[] {
                             new ScopePermission { Resource = "Data", Scope = "Birthdate"},
                             new ScopePermission { Resource = "Data", Scope = "Country"},
                             new ScopePermission { Resource = "Data", Scope = "Email", IsEnabled = true},
                             new ScopePermission { Resource = "Data", Scope = "FullName", IsEnabled = true},
                             new ScopePermission { Resource = "Data", Scope = "Gender"},
                             new ScopePermission { Resource = "Data", Scope = "Language"},
                             new ScopePermission { Resource = "Data", Scope = "Nickname"},
                             new ScopePermission { Resource = "Data", Scope = "PostalCode"},
                             new ScopePermission { Resource = "Data", Scope = "TimeZone"},
                         };
        }
    }
}