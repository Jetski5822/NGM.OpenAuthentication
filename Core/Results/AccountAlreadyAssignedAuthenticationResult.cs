namespace NGM.OpenAuthentication.Core.Results {
    public class AccountAlreadyAssignedAuthenticationResult : AuthenticationResult {
        public AccountAlreadyAssignedAuthenticationResult() : base (Statuses.ErrorAuthenticating) {}
    }
}