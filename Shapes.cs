using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {

        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn") {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_LogOn");
            }
            if (context.ShapeType == "Register") {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_Register");
            }
        }
    }
}