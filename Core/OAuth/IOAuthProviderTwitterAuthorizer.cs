using LinqToTwitter;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderTwitterAuthorizer : IOAuthProviderAuthorizer {
        ITwitterAuthorizer GetAuthorizer(IUser user);
    }
}