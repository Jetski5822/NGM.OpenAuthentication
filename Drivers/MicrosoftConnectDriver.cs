using JetBrains.Annotations;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Drivers {
    [UsedImplicitly]
    [OrchardFeature("MicrosoftConnect")]
    public class MicrosoftConnectContactListPartDriver : ContentPartDriver<MicrosoftConnectSignInPart> {
        protected override DriverResult Display(MicrosoftConnectSignInPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("MicrosoftConnectSignIn", () => shapeHelper.MicrosoftConnectSignIn(Model: part));
        }
    }
}