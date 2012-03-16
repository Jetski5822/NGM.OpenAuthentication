using Orchard;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        private readonly WorkContext _workContext;

        public Shapes(IWorkContextAccessor workContextAccessor) {
            _workContext = workContextAccessor.GetContext();
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn" || context.ShapeType == "Register") {
                if ((_workContext.HttpContext.Session["parameters"] as Core.OpenAuthenticationParameters) != null)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_AssociateMessage");
            }
        }
    }
}