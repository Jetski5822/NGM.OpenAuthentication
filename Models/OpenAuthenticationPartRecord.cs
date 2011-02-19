using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationPartRecord : ContentPartRecord {
        public virtual int UserId { get; set; }
        public virtual string ExternalIdentifier { get; set; }
        public virtual string ExternalDisplayIdentifier { get; set; }
    }
}