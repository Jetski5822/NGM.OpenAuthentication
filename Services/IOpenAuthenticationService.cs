using NGM.OpenAuthentication.Core;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService {
        IUser GetUser(OpenIdIdentifier openIdIdentifier);
    }
}