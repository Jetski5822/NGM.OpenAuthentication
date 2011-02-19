using System.Linq;
using JetBrains.Annotations;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Security;

namespace NGM.OpenAuthentication.Drivers {
    [UsedImplicitly]
    public class OpenAuthenticationPartDriver : ContentPartDriver<OpenAuthenticationPart> {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private const string TemplateName = "Parts/Accounts.UserOpenAuthentication";

        public OpenAuthenticationPartDriver(IAuthenticationService authenticationService,
            IAuthorizationService authorizationService,
            IOpenAuthenticationService openAuthenticationService) {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _openAuthenticationService = openAuthenticationService;
            T = NullLocalizer.Instance;
        }

        protected override string Prefix {
            get {
                return "OpenAuthentication";
            }
        }

        public Localizer T { get; set; }

        protected override DriverResult Editor(OpenAuthenticationPart openAuthenticationPart, dynamic shapeHelper) {
            var user = _authenticationService.GetAuthenticatedUser();

            if (!_authorizationService.TryCheckAccess(StandardPermissions.SiteOwner, user, openAuthenticationPart))
                return null;

            return ContentShape("Parts_Accounts_UserOpenAuthentication_Edit",
                () => {
                    var entries =
                        _openAuthenticationService
                            .GetExternalIdentifiersFor(user)
                            .List()
                            .ToList()
                            .Select(account => CreateAccountEntry(account.Record));

                    if (entries.ToList().Count.Equals(0)) return null;

                    var viewModel = new OpenIdIndexViewModel {
                        Accounts = entries.ToList()
                    };

                    return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: viewModel, Prefix: Prefix);
                });
        }

        protected override DriverResult Editor(OpenAuthenticationPart openAuthenticationPart, IUpdateModel updater, dynamic shapeHelper) {
            // don't apply editor without apply roles permission
            if (!_authorizationService.TryCheckAccess(StandardPermissions.SiteOwner, _authenticationService.GetAuthenticatedUser(), openAuthenticationPart))
                return null;

            return Editor(openAuthenticationPart, shapeHelper);
        }

        private AccountEntry CreateAccountEntry(OpenAuthenticationPartRecord openAuthenticationPart) {
            return new AccountEntry {
                Account = openAuthenticationPart
            };
        }
    }
}