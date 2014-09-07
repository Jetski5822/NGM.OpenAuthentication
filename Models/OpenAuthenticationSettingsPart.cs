using Orchard.ContentManagement;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationSettingsPart : ContentPart<OpenAuthenticationSettingsPartRecord> {
        public bool AutoRegistrationEnabled {
            get { return this.Retrieve(x => x.AutoRegistrationEnabled); }
            set { this.Store(x => x.AutoRegistrationEnabled, value); }
        }
    }
}