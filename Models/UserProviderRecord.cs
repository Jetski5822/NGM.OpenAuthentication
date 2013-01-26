namespace NGM.OpenAuthentication.Models {
    public class UserProviderRecord {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual string ProviderName { get; set; }
        public virtual string ProviderUserId { get; set; }
    }
}