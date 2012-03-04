using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Facebook {
    [OrchardFeature("Facebook")]
    public class FacebookShapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (Shapes.IsLogOn(context) || Shapes.IsCreate(context)) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_Facebook_LogOn");
            }
        }
    }
}