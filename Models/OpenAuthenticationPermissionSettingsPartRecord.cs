using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationPermissionSettingsPartRecord : ContentPartRecord {
        public virtual string NamedPermission { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual int HashedProvider { get; set; }
    }
}