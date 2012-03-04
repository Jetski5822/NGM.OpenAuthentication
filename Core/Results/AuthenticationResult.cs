using System.Collections.Generic;

namespace NGM.OpenAuthentication.Core.Results {
    public class AuthenticationResult {
        public AuthenticationResult(Statuses statuses) {
            Status = statuses;
        }

        public AuthenticationResult(Statuses statuses, KeyValuePair<string, string> error) : this(statuses) {
            Error = error;
        }

        public Statuses Status { get; private set; }

        public KeyValuePair<string, string> Error { get; private set; }
    }
}