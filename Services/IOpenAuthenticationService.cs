using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService : IDependency {
        bool IsAccountExists(string identifier);
        
        void AssociateOpenIdWithUser(IUser user, string openIdIdentifier, string friendlyOpenIdIdentifier);

        IUser GetUser(string openIdIdentifier);

        OpenAuthenticationSettingsPart GetSettings();

        IContentQuery<OpenAuthenticationPart, OpenAuthenticationPartRecord> GetIdentifiersFor(IUser user);
        void RemoveOpenIdAssociation(string openIdIdentifier);
    }
}