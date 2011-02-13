using Orchard.ContentManagement.Records;

namespace NGM.OpenAuthentication.Models {
    public class OpenAuthenticationSettingsPartRecord : ContentPartRecord {
        public virtual bool OpenIdEnabled { get; set; }
        public virtual bool CardSpaceEnabled { get; set; }
        public virtual bool Birthdate { get; set; }
        public virtual bool Country { get; set; }
        public virtual bool Email { get; set; }
        public virtual bool FullName { get; set; }
        public virtual bool Gender { get; set; }
        public virtual bool Language { get; set; }
        public virtual bool Nickname { get; set; }
        public virtual bool PostalCode { get; set; }
        public virtual bool TimeZone { get; set;}
    }
}