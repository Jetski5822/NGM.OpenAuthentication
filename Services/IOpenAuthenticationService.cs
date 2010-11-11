using NGM.OpenAuthentication.Models;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService {
        bool IsAccountExists(string identifier);
        void AssociateOpenIdWithUser(IUser user, string openIdIdentifier);
        IUser CreateUser(RegisterModel openIdIdentifier);
        IUser GetUser(string openIdIdentifier);
        OpenAuthenticationSettingsRecord GetSettings();
    }
}