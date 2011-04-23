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
}