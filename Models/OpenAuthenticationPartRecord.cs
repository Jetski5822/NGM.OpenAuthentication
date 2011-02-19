using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationPartRecord : ContentPartRecord {
        public virtual int UserId { get; set; }
        public virtual string ExternalIdentifier { get; set; }
        public virtual string ExternalDisplayIdentifier { get; set; }
        public virtual string OAuthToken { get; set; }
        public virtual string OAuthAccessToken { get; set; }
        public virtual int HashedProvider { get; set; }
    }
}