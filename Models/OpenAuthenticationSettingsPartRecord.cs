using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationSettingsPartRecord : ContentPartRecord {
        public virtual bool AutoRegistrationEnabled { get; set; }
    }
}