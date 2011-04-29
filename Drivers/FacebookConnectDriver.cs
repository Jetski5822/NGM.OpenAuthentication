using JetBrains.Annotations;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Drivers {
    [UsedImplicitly]
    [OrchardFeature("Facebook")]
    public class FacebookConnectDriver : ContentPartDriver<FacebookConnectSignInPart> {
        protected override DriverResult Display(FacebookConnectSignInPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("FacebookConnectSignIn", () => shapeHelper.FacebookConnectSignIn(Model: part));
        }
    }
}