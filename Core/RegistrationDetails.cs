namespace NGM.OpenAuthentication.Core {
    public struct RegistrationDetails {
        public RegistrationDetails(OpenAuthenticationParameters parameters)
            : this() {
            foreach (var claim in parameters.UserClaims) {
                if (string.IsNullOrEmpty(EmailAddress)) {
                    EmailAddress = claim.Contact.Email;
                    UserName = claim.Contact.Email;
                }
            }
        }

        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public bool IsValid() {
            return !string.IsNullOrEmpty(UserName) && (!string.IsNullOrEmpty(EmailAddress));
        }
    }
}