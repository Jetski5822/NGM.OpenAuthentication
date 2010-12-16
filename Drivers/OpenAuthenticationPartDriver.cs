using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Security;

namespace NGM.OpenAuthentication.Drivers {
    public class OpenAuthenticationPartDriver : ContentPartDriver<OpenAuthenticationPart> {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;

        public OpenAuthenticationPartDriver(IAuthenticationService authenticationService,
            IAuthorizationService authorizationService) {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            T = NullLocalizer.Instance;
        }

        protected override string Prefix {
            get {
                return "OpenAuthentication";
            }
        }

        public Localizer T { get; set; }

        protected override DriverResult Editor(OpenAuthenticationPart userRolesPart, dynamic shapeHelper) {
            // don't show editor without apply roles permission
            if (!_authorizationService.TryCheckAccess(StandardPermissions.SiteOwner, _authenticationService.GetAuthenticatedUser(), userRolesPart))
                return null;

            return null;
        }

        protected override DriverResult Editor(OpenAuthenticationPart userRolesPart, IUpdateModel updater, dynamic shapeHelper) {
            // don't apply editor without apply roles permission
            if (!_authorizationService.TryCheckAccess(StandardPermissions.SiteOwner, _authenticationService.GetAuthenticatedUser(), userRolesPart))
                return null;

            return null;
        }
    }
}