using NGM.OpenAuthentication.Providers.Facebook.Services;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Facebook {
    [OrchardFeature("Facebook")]
    public class Shapes : IShapeFactoryEvents {
        private readonly IOAuthProviderFacebookAuthenticator _oAuthProviderFacebookAuthenticator;
        private readonly IAuthenticationShapeHelper _authenticationShapeHelper;

        public Shapes(IOAuthProviderFacebookAuthenticator oAuthProviderFacebookAuthenticator, IAuthenticationShapeHelper authenticationShapeHelper) {
            _oAuthProviderFacebookAuthenticator = oAuthProviderFacebookAuthenticator;
            _authenticationShapeHelper = authenticationShapeHelper;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if ((_authenticationShapeHelper.IsLogOn(context) || _authenticationShapeHelper.IsCreate(context)) && _oAuthProviderFacebookAuthenticator.IsConsumerConfigured) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_Facebook_LogOn");
            }
        }
    }
}