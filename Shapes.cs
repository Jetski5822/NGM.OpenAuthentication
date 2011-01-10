using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {

        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn") {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_LogOn");
            }
        }
    }
}