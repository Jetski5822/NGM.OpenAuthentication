using System;
using System.Web;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

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

    [OrchardFeature("OpenId")]
    public class OpenIdShapes : IShapeFactoryEvents {
        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (Shapes.IsLogOn(context) || Shapes.IsCreate(context)) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_LogOn");
            }
        }
    }

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