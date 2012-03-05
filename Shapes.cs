using System;
using System.Web;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn" || context.ShapeType == "Register") {
                if ((HttpContext.Current.Session["parameters"] as Core.OpenAuthenticationParameters) != null)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_AssociateMessage");
            }
        }

        public static bool IsLogOn(ShapeCreatedContext context) {
            return context.ShapeType == "LogOn";
        }

        public static bool IsCreate(ShapeCreatedContext context) {
            if (context.ShapeType == "Create" && 
                HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString().Equals("Admin", StringComparison.InvariantCultureIgnoreCase) &&
                HttpContext.Current.Request.RequestContext.RouteData.Values["Area"].ToString().Equals("NGM.OpenAuthentication", StringComparison.InvariantCultureIgnoreCase)) {
                return true;
            }
            return false;
        }
    }
}