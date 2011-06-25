namespace NGM.OpenAuthentication.Models {
    public class ScopeProviderPermissionRecord {
        public virtual int Id { get; set; }
        public virtual string Resource { get; set; }
        public virtual string Scope { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual string HashedProvider { get; set; }
    }
}