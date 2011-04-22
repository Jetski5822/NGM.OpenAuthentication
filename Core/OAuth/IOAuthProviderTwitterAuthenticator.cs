using LinqToTwitter;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderTwitterAuthenticator : IOAuthProviderAuthenticator {
        ITwitterAuthorizer GetAuthorizer(IUser user);
    }
}