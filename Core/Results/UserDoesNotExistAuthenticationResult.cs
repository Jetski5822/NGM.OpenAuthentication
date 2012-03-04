using System.Collections.Generic;

namespace NGM.OpenAuthentication.Core.Results {
    public class UserDoesNotExistAuthenticationResult : AuthenticationResult {
        public UserDoesNotExistAuthenticationResult():base (Statuses.UserDoesNotExist,
                                                            new KeyValuePair<string, string>("AccessDenied", "User does not exist on system")) {}
    }
}