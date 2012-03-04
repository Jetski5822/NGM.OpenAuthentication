namespace NGM.OpenAuthentication.Core.Results {
    public class AuthenticationResult {
        public AuthenticationResult(Statuses statuses) {
            Status = statuses;
        }

        public Statuses Status { get; private set; }
    }
}