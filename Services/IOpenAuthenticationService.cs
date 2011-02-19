using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService : IDependency {
        bool AccountExists(string externalIdentifier);
        
        void AssociateExternalAccountWithUser(IUser user, string externalIdentifier, string externalDisplayIdentifier);

        IUser GetUser(string externalIdentifier);

        OpenAuthenticationSettingsPart GetSettings();

        IContentQuery<OpenAuthenticationPart, OpenAuthenticationPartRecord> GetExternalIdentifiersFor(IUser user);
        void RemoveOpenIdAssociation(string externalIdentifier);
    }
}