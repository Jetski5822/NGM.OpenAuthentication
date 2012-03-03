using LinqToTwitter;
using NGM.OpenAuthentication.Core.OAuth;
using Orchard.Security;

namespace NGM.OpenAuthentication.Provider.Twitter.Services {
    public interface IOAuthProviderTwitterAuthenticator : IOAuthProviderAuthenticator {
        ITwitterAuthorizer GetAuthorizer(IUser user);
    }
}