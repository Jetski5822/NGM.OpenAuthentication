namespace NGM.OpenAuthentication.Models {
    public class RegisterModel {

        public RegisterModel(string identifier, string email) {
            Identifier = identifier;
            Email = email;
        }

        public string Identifier { get; set; }
        public string Email { get; set;}
    }
}