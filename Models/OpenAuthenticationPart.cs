using NGM.OpenAuthentication.Core;
using Orchard.ContentManagement;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationPart : ContentPart<OpenAuthenticationPartRecord> {
        public int UserId {
            get { return Record.UserId; }
            set { Record.UserId = value; }
        }

        public string ExternalIdentifier {
            get { return Record.ExternalIdentifier; }
            set { Record.ExternalIdentifier = value; }
        }

        public string ExternalDisplayIdentifier {
            get { return Record.ExternalDisplayIdentifier; }
            set { Record.ExternalDisplayIdentifier = value; }
        }

        public string OAuthToken {
            get { return Record.OAuthToken; }
            set { Record.OAuthToken = value; }
        }

        public string OAuthAccessToken {
            get { return Record.OAuthAccessToken; }
            set { Record.OAuthAccessToken = value; }
        }

        public string HashedProvider {
            get { return Record.HashedProvider; }
            set { Record.HashedProvider = value; }
        }

        public IAccessControlProvider Provider { get; set; }
    }
}
