using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace NGM.OpenAuthentication {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("Users"),
                menu => menu.Add(T("Associated Accounts"), "3.0", item => item.Action("Index", "Admin", new { area = "NGM.OpenAuthentication" })
                    .LocalNav().Permission(StandardPermissions.AccessAdminPanel)));
        }
    }

    ////http://developers.facebook.com/docs/authentication/permissions/
    //[OrchardFeature("Facebook")]
    //public class FacebookAdminPermissionsMenu : INavigationProvider {
    //    public Localizer T { get; set; }
    //    public string MenuName { get { return "admin"; } }

    //    public void GetNavigation(NavigationBuilder builder) {
    //        builder.Add(T("Users"),
    //            menu => menu.Add(T("Facebook Permissions"), "4.0", item => item.Action("Index", "AdminPermissions", new { area = "NGM.OpenAuthentication", provider = "Facebook" })
    //                .LocalNav().Permission(StandardPermissions.SiteOwner)));
    //    }
    //}
}
