using System.Collections.Generic;

namespace NGM.OpenAuthentication.Core.Results {
    public class AccountAlreadyAssignedAuthenticationResult : AuthenticationResult {
        public AccountAlreadyAssignedAuthenticationResult() : base (OpenAuthenticationStatus.ErrorAuthenticating, 
                                                                    new KeyValuePair<string, string>("AccountAssigned", "Account is already assigned")) {}
    }
}