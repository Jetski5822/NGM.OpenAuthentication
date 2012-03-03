using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Provider.Twitter {
    [OrchardFeature("Twitter")]
    public class TwitterShapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (Shapes.IsLogOn(context) || Shapes.IsCreate(context)) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_Twitter_LogOn");
            }
        }
    }
}