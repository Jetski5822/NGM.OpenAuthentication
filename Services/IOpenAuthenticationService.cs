using System.Collections.Generic;
using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService : IDependency {
        bool IsAccountExists(string identifier);
        
        void AssociateOpenIdWithUser(IUser user, string openIdIdentifier);
        
        IUser CreateUser(RegisterModel openIdIdentifier);
        IUser GetUser(string openIdIdentifier);
        
        OpenAuthenticationSettingsRecord GetSettings();

        IEnumerable<string> GetIdentifiersFor(IUser user);
    }
}