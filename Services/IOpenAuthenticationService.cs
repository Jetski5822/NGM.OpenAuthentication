using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService {
        IUser GetUserFor(OpenIdIdentifier openIdIdentifier);
        void AssociateOpenIdWithUser(IUser user, OpenIdIdentifier openIdIdentifier, ExtendedUserPropertiesContext extendedUserPropertiesContext)
    }
}