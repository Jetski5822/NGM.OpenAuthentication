using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OAuthProviderSettingsPartRecord : ContentPartRecord {
        public virtual string Provider { get; set; }
        public virtual string ClientIdentifier { get; set; }
        public virtual string ClientSecret { get; set; }
    }
}