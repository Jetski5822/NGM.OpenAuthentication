using LinqToTwitter;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthTwitterAuthorizer : IOAuthAuthorizer {
        ITwitterAuthorizer GetAuthorizer(IUser user);
    }
}