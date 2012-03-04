using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Twitter {
    [OrchardFeature("Twitter")]
    public class Shapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (OpenAuthentication.Shapes.IsLogOn(context) || OpenAuthentication.Shapes.IsCreate(context)) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_Twitter_LogOn");
            }
        }
    }
}