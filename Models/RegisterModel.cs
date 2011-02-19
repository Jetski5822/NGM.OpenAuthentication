using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Models {
    public class RegisterModel {
        public RegisterModel() {
            // MVC Requires this.
        }

        public RegisterModel(OpenAuthenticationParameters parameters) {
            UserName = parameters.ExternalDisplayIdentifier;
            Parameters = parameters;
        }

        public OpenAuthenticationParameters Parameters { get; set; }
        public string UserName { get; set; }
        public string Email { get; set;}
    }
}