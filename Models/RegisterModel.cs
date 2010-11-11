namespace NGM.OpenAuthentication.Models {
    public class RegisterModel {
        public RegisterModel(string identifier) {
            Identifier = identifier;
        }

        public string ReturnUrl { get; set; }
        public string Identifier { get; set; }
        public string Email { get; set;}
    }
}