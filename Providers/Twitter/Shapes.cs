using NGM.OpenAuthentication.Providers.Twitter.Services;
using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Twitter {
    [OrchardFeature("Twitter")]
    public class Shapes : IShapeFactoryEvents {
        private readonly IOAuthProviderTwitterAuthenticator _oAuthProviderTwitterAuthenticator;

        public Shapes(IOAuthProviderTwitterAuthenticator oAuthProviderTwitterAuthenticator) {
            _oAuthProviderTwitterAuthenticator = oAuthProviderTwitterAuthenticator;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if ((OpenAuthentication.Shapes.IsLogOn(context) || OpenAuthentication.Shapes.IsCreate(context)) && _oAuthProviderTwitterAuthenticator.IsConsumerConfigured) {
                context.Shape.Metadata.Content.Add("Wrappers_Account_Twitter_LogOn");
            }
        }
    }
}