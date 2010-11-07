using DotNetOpenAuth.OpenId.RelyingParty;

namespace NGM.OpenAuthentication.Models {
    public class RegisterModel {
        public RegisterModel(string identifier) {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}