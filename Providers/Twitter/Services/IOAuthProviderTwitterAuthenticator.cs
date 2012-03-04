using LinqToTwitter;
using NGM.OpenAuthentication.Core.OAuth;
using Orchard.Security;

namespace NGM.OpenAuthentication.Providers.Twitter.Services {
    public interface IOAuthProviderTwitterAuthenticator : IOAuthProviderAuthenticator {
        ITwitterAuthorizer GetAuthorizer(IUser user);
    }
}