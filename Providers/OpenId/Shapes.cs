using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.OpenId {
    [OrchardFeature("OpenId")]
    public class Shapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (OpenAuthentication.Shapes.IsLogOn(context) || OpenAuthentication.Shapes.IsCreate(context)) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_LogOn");
            }
        }
    }
}