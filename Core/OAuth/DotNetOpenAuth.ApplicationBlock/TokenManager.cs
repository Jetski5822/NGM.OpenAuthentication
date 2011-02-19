using System;
using DotNetOpenAuth.OAuth2;

namespace NGM.OpenAuthentication.Core.OAuth.DotNetOpenAuth.ApplicationBlock {
    public class TokenManager : IClientAuthorizationTracker {
        public IAuthorizationState GetAuthorizationState(Uri callbackUrl, string clientState) {
            return new AuthorizationState {
                Callback = callbackUrl,
            };
        }
    }
}