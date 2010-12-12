using Orchard.ContentManagement;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationPart : ContentPart<OpenAuthenticationPartRecord> {
        public string ClaimedIdentifier {
            get { return Record.ClaimedIdentifier; }
            set { Record.ClaimedIdentifier = value; }
        }

        public string FriendlyIdentifierForDisplay {
            get { return Record.FriendlyIdentifierForDisplay; }
            set { FriendlyIdentifierForDisplay = value; }
        }
    }
}
