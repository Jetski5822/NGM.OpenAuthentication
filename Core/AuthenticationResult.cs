using System.Collections.Generic;

namespace NGM.OpenAuthentication.Core {
    public class AuthenticationResult {
        public AuthenticationResult(OpenAuthenticationStatus status) {
            Status = status;
        }

        public AuthenticationResult(OpenAuthenticationStatus status, KeyValuePair<string, string> error) : this(status) {
            Error = error;
        }

        public OpenAuthenticationStatus Status { get; private set; }

        public KeyValuePair<string, string> Error { get; private set; }
    }
}