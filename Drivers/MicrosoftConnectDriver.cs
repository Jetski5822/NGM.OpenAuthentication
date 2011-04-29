using System.Linq;
using System.Text;
using JetBrains.Annotations;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Drivers {
    [UsedImplicitly]
    [OrchardFeature("MicrosoftConnect")]
    public class MicrosoftConnectDriver : ContentPartDriver<MicrosoftConnectSignInPart> {
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;

        public MicrosoftConnectDriver(IScopeProviderPermissionService scopeProviderPermissionService) {
            _scopeProviderPermissionService = scopeProviderPermissionService;
        }

        protected override DriverResult Display(MicrosoftConnectSignInPart part, string displayType, dynamic shapeHelper) {
            var extendedPermissions = _scopeProviderPermissionService.Get(Provider.LiveId).Where(o => o.IsEnabled).Select(o => o.Scope).ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var extendedPermission in extendedPermissions) {
                stringBuilder.Append(extendedPermission);
                stringBuilder.Append(",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return ContentShape("MicrosoftConnectSignIn", () => shapeHelper.MicrosoftConnectSignIn(Model: part, Permissions: stringBuilder.ToString()));
        }
    }
}