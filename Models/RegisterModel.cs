namespace NGM.OpenAuthentication.Models {
    public class RegisterModel {
        public string ReturnUrl { get; set; }
        public string ClaimedIdentifier { get; set; }
        public string FriendlyIdentifier { get; set; }
        public string Email { get; set;}
    }
}