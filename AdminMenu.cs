using NGM.OpenAuthentication.Services;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace NGM.OpenAuthentication {
    public class AdminMenu : INavigationProvider {
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public AdminMenu(IOpenAuthenticationService openAuthenticationService) {
            _openAuthenticationService = openAuthenticationService;
        }

        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            if (_openAuthenticationService.GetSettings().Record.OpenIdEnabled)
                builder.Add(T("Users"), "40",
                    menu => menu.Add(T("Associated Accounts"), "3.0", item => item.Action("Index", "Admin", new { area = "NGM.OpenAuthentication" })
                        .Permission(StandardPermissions.AccessAdminPanel)));
        }
    }
}
