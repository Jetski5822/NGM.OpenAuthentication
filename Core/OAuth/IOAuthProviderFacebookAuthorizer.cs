using Facebook;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderFacebookAuthorizer : IOAuthProviderAuthorizer {
        FacebookClient GetClient(IUser user);
    }
}