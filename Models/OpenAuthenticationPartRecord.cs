using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationPartRecord : ContentPartRecord {
        public virtual string ClaimedIdentifier { get; set; }
        public virtual string FriendlyIdentifierForDisplay { get; set; }
    }
}