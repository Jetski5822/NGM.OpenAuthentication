using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationPermissionSettingsPartRecord : ContentPartRecord {
        public virtual string Scope { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual int HashedProvider { get; set; }
    }
}