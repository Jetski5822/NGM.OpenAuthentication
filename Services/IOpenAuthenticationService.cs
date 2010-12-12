using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService : IDependency {
        bool IsAccountExists(string identifier);
        
        void AssociateOpenIdWithUser(IUser user, string openIdIdentifier, string friendlyOpenIdIdentifier);
        
        IUser CreateUser(RegisterModel openIdIdentifier);
        IUser GetUser(string openIdIdentifier);
        
        OpenAuthenticationSettingsRecord GetSettings();

        IContentQuery<OpenAuthenticationPart> GetIdentifiersFor(IUser user);
    }
}