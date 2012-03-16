using System;
using Orchard;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class AuthenticationShapeHelper : IAuthenticationShapeHelper {
        private readonly WorkContext _workContext;

        public AuthenticationShapeHelper(IWorkContextAccessor workContextAccessor) {
            _workContext = workContextAccessor.GetContext();
        }

        public bool IsLogOn(ShapeCreatedContext context) {
            return context.ShapeType == "LogOn";
        }

        public bool IsCreate(ShapeCreatedContext context) {
            if (context.ShapeType == "Create" &&
                _workContext.HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString().Equals("Admin", StringComparison.InvariantCultureIgnoreCase) &&
                _workContext.HttpContext.Request.RequestContext.RouteData.Values["Area"].ToString().Equals("NGM.OpenAuthentication", StringComparison.InvariantCultureIgnoreCase)) {
                return true;
            }
            return false;
        }
    }
}