using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.OpenId {
    [OrchardFeature("OpenId")]
    public class Shapes : IShapeFactoryEvents {
        private readonly IAuthenticationShapeHelper _authenticationShapeHelper;

        public Shapes(IAuthenticationShapeHelper authenticationShapeHelper) {
            _authenticationShapeHelper = authenticationShapeHelper;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (_authenticationShapeHelper.IsLogOn(context) || _authenticationShapeHelper.IsCreate(context)) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_LogOn");
            }
        }
    }
}