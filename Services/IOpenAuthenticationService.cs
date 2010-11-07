using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService {
        void AssociateOpenIdWithUser(IUser user, string openIdIdentifier);
        IUser CreateUser(string openIdIdentifier);
        IUser GetUser(string openIdIdentifier);
    }
}