using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationSettingsPartRecord : ContentPartRecord {
        public virtual string FacebookClientIdentifier { get; set; }
        public virtual string FacebookClientSecret { get; set; }
        public virtual string TwitterClientIdentifier { get; set; }
        public virtual string TwitterClientSecret { get; set; }
        public virtual string LiveIdClientIdentifier { get; set; }
        public virtual string LiveIdClientSecret { get; set; }

        public virtual bool AutoRegisterEnabled { get; set; }
    }
}