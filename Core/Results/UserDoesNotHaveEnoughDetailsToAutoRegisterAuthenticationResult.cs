namespace NGM.OpenAuthentication.Core.Results {
    public class UserDoesNotHaveEnoughDetailsToAutoRegisterAuthenticationResult : AuthenticationResult {
        public UserDoesNotHaveEnoughDetailsToAutoRegisterAuthenticationResult() : base(Statuses.AssociateOnLogon) {
        }
    }
}