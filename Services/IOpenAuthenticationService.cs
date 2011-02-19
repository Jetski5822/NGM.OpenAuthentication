using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOpenAuthenticationService : IDependency {
        bool AccountExists(OpenAuthenticationParameters parameters);
        
        void AssociateExternalAccountWithUser(IUser user, OpenAuthenticationParameters parameters);

        IUser GetUser(OpenAuthenticationParameters parameters);

        OpenAuthenticationSettingsPart GetSettings();

        IContentQuery<OpenAuthenticationPart, OpenAuthenticationPartRecord> GetExternalIdentifiersFor(IUser user);
        void RemoveAssociation(OpenAuthenticationParameters parameters);
    }
}