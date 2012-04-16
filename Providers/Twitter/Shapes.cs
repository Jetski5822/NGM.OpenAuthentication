using NGM.OpenAuthentication.Providers.Twitter.Services;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Twitter {
    [OrchardFeature("Authentication.Twitter")]
    public class Shapes : IShapeFactoryEvents {
        private readonly IOAuthProviderTwitterAuthenticator _oAuthProviderTwitterAuthenticator;
        private readonly IAuthenticationShapeHelper _authenticationShapeHelper;

        public Shapes(IOAuthProviderTwitterAuthenticator oAuthProviderTwitterAuthenticator, IAuthenticationShapeHelper authenticationShapeHelper) {
            _oAuthProviderTwitterAuthenticator = oAuthProviderTwitterAuthenticator;
            _authenticationShapeHelper = authenticationShapeHelper;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if ((_authenticationShapeHelper.IsLogOn(context) || _authenticationShapeHelper.IsCreate(context)) && _oAuthProviderTwitterAuthenticator.IsConsumerConfigured) {
                context.Shape.Metadata.Content.Add("Wrappers_Account_Twitter_LogOn");
            }
        }
    }
}