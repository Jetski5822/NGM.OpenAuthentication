using System;
using Orchard;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        private readonly IOrchardServices _orchardServices;

        public Shapes(IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn" || context.ShapeType == "Register") {
                if ((_orchardServices.WorkContext.HttpContext.Session["parameters"] as Core.OpenAuthenticationParameters) != null)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_AssociateMessage");
            }
        }

        public bool IsLogOn(ShapeCreatedContext context) {
            return context.ShapeType == "LogOn";
        }

        public bool IsCreate(ShapeCreatedContext context) {
            if (context.ShapeType == "Create" &&
                _orchardServices.WorkContext.HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString().Equals("Admin", StringComparison.InvariantCultureIgnoreCase) &&
                _orchardServices.WorkContext.HttpContext.Request.RequestContext.RouteData.Values["Area"].ToString().Equals("NGM.OpenAuthentication", StringComparison.InvariantCultureIgnoreCase)) {
                return true;
            }
            return false;
        }
    }
}