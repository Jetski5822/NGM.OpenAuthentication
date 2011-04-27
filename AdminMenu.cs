using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace NGM.OpenAuthentication {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("Users"), BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu) {
            menu.Add(T("Associated Accounts"), "3.0", item => item.Action("Index", "Admin", new {area = "NGM.OpenAuthentication"})
                .LocalNav().Permission(StandardPermissions.AccessAdminPanel));

            menu.Add(T("Provider Permissions"), "4.0", item => item.Action("Edit", "AdminPermission", new {area = "NGM.OpenAuthentication"})
                .LocalNav().Permission(StandardPermissions.SiteOwner));
        }
    }
}
