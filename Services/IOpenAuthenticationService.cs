using NGM.OpenAuthentication.Core;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService {
        IUser GetUserFor(OpenIdIdentifier openIdIdentifier);
    }
}