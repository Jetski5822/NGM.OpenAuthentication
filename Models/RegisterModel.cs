namespace NGM.OpenAuthentication.Models {
    public class RegisterModel {
        public string UserName { get; set; }
        public string ExternalIdentifier { get; set; }
        public string ExternalDisplayIdentifier { get; set; }
        public string Email { get; set;}
    }
}