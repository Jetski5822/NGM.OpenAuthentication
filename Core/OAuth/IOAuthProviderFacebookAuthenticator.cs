using Facebook;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderFacebookAuthenticator : IOAuthProviderAuthenticator {
        FacebookClient GetClient(IUser user);
    }
}