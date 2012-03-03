using Facebook;
using NGM.OpenAuthentication.Core.OAuth;
using Orchard.Security;

namespace NGM.OpenAuthentication.Facebook.Services {
    public interface IOAuthProviderFacebookAuthenticator : IOAuthProviderAuthenticator {
        FacebookClient GetClient(IUser user);
    }
}