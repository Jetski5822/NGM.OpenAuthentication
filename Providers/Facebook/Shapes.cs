using NGM.OpenAuthentication.Providers.Facebook.Services;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Facebook {
    [OrchardFeature("Facebook")]
    public class Shapes : IShapeFactoryEvents {
        private readonly IOAuthProviderFacebookAuthenticator _oAuthProviderFacebookAuthenticator;

        public Shapes(IOAuthProviderFacebookAuthenticator oAuthProviderFacebookAuthenticator) {
            _oAuthProviderFacebookAuthenticator = oAuthProviderFacebookAuthenticator;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if ((OpenAuthentication.Shapes.IsLogOn(context) || OpenAuthentication.Shapes.IsCreate(context)) && _oAuthProviderFacebookAuthenticator.IsConsumerConfigured) {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_Facebook_LogOn");
            }
        }
    }
}