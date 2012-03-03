using System.Collections.Generic;
using NGM.MicrosoftConnect;
using NGM.OpenAuthentication.Core;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;

namespace NGM.OpenAuthentication.Provider.MicrosoftConnect.Permissions {
    [OrchardFeature("MicrosoftConnect")]
    public class MicrosoftConnectScopePermissions : IScopePermissionProvider {
        public virtual Feature Feature { get; set; }

        public AccessControlProvider Provider {
            get { return new MicrosoftConnectAccessControlProvider(); }
        }

        //http://msdn.microsoft.com/en-us/library/hh243646.aspx
        public IEnumerable<ScopePermission> GetPermissions() {
            return new[] {
                             new ScopePermission {Resource = Scope.WLBasic.Resource, Scope = Scope.WLBasic.ScopeName, IsEnabled = true},
                             new ScopePermission {Resource = "Core", Scope = "wl.offline_access"},
                             new ScopePermission {Resource = "Core", Scope = "wl.signin", IsEnabled = true},
                             new ScopePermission {Resource = "Extended", Scope = "wl.birthday"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.contacts_birthday"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.contacts_photos"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.emails"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.events_create"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.phone_numbers"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.photos"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.postal_addresses"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.share"},
                             new ScopePermission {Resource = "Extended", Scope = "wl.work_profile"},
                             new ScopePermission {Resource = "Developer", Scope = "wl.applications"},
                             new ScopePermission {Resource = "Developer", Scope = "wl.applications_create"},
                         };
        }
    }
}