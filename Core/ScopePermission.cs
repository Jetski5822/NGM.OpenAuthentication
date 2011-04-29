namespace NGM.OpenAuthentication.Core {
    public class ScopePermission {
        public string Resource { get; set; }
        public string Scope { get; set; }
        public string Description { get; set; }
        public Provider Provider { get; set; }
        public bool IsEnabled { get; set; }
    }
}                   