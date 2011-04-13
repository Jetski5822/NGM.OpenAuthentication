using Orchard.ContentManagement;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationSettingsPart : ContentPart<OpenAuthenticationSettingsPartRecord> {
        public string FacebookClientIdentifier
        {
            get { return Record.FacebookClientIdentifier; }
            set { Record.FacebookClientIdentifier = value; }
        }
    }
}