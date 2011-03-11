using System.Collections.Generic;

namespace NGM.OpenAuthentication.Core {
    public class AuthorizationResult {
        public AuthorizationResult(OpenAuthenticationStatus status) {
            Status = status;
        }

        public AuthorizationResult(OpenAuthenticationStatus status, KeyValuePair<string, string> error) : this(status) {
            Error = error;
        }

        public OpenAuthenticationStatus Status { get; private set; }

        public KeyValuePair<string, string> Error { get; private set; }
    }
}