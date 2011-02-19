using System;
using DotNetOpenAuth.OAuth2;

namespace NGM.OpenAuthentication.Core.OAuth.DotNetOpenAuth.ApplicationBlock.Facebook {
    public class FacebookClient : WebServerClient {
        private static readonly AuthorizationServerDescription FacebookDescription = new AuthorizationServerDescription {
            TokenEndpoint = new Uri("https://graph.facebook.com/oauth/access_token"),
            AuthorizationEndpoint = new Uri("https://graph.facebook.com/oauth/authorize"),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        public FacebookClient()
            : base(FacebookDescription) {
            this.AuthorizationTracker = new TokenManager();
        }
    }
}