using System.Collections.Generic;

namespace NGM.OpenAuthentication.Core {
    public class AuthenticationResult {
        public AuthenticationResult(OpenAuthenticationStatus openAuthenticationStatus) {
            Status = openAuthenticationStatus;
        }

        public AuthenticationResult(OpenAuthenticationStatus openAuthenticationStatus, KeyValuePair<string, string> error) : this(openAuthenticationStatus) {
            Error = error;
        }

        public OpenAuthenticationStatus Status { get; private set; }

        public KeyValuePair<string, string> Error { get; private set; }
    }

    public class AuthenticatedAuthenticationResult : AuthenticationResult {
        public AuthenticatedAuthenticationResult() : base (OpenAuthenticationStatus.Authenticated) {}
    }

    public class AccountAlreadyAssignedAuthenticationResult : AuthenticationResult {
        public AccountAlreadyAssignedAuthenticationResult() : base (OpenAuthenticationStatus.ErrorAuthenticating, 
                    new KeyValuePair<string, string>("AccountAssigned", "Account is already assigned")) {}
    }

    public class UserDoesNotExistAuthenticationResult : AuthenticationResult {
        public UserDoesNotExistAuthenticationResult():base (OpenAuthenticationStatus.UserDoesNotExist,
                            new KeyValuePair<string, string>("AccessDenied", "User does not exist on system")) {}
    }

    public class UserDoesNotHaveEnoughDetailsToAutoRegisterAuthenticationResult : AuthenticationResult {
        public UserDoesNotHaveEnoughDetailsToAutoRegisterAuthenticationResult() : base (OpenAuthenticationStatus.AssociateOnLogon,
                            new KeyValuePair<string, string>("AccessDenied", "User does not have enough details to auto create account")) {}
    }
}