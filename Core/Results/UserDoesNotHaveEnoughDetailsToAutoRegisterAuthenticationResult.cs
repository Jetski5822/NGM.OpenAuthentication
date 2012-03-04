using System.Collections.Generic;

namespace NGM.OpenAuthentication.Core.Results {
    public class UserDoesNotHaveEnoughDetailsToAutoRegisterAuthenticationResult : AuthenticationResult {
        public UserDoesNotHaveEnoughDetailsToAutoRegisterAuthenticationResult() : base (Statuses.AssociateOnLogon,
                                                                                        new KeyValuePair<string, string>("AccessDenied", "User does not have enough details to auto create account")) {}
    }
}