using System.Collections.Generic;
using NGM.OpenAuthentication.Core.Claims;

namespace NGM.OpenAuthentication.Core {
    public abstract class OpenAuthenticationParameters {
        public abstract Provider Provider { get; }
        public string ExternalIdentifier { get; set; }
        public string ExternalDisplayIdentifier { get; set; }
        public string OAuthToken { get; set; }
        public string OAuthAccessToken { get; set; }

        public virtual IList<UserClaims> UserClaims {
            get { return new List<UserClaims>(0); }
        }

        public virtual string HashedProvider {
            get { return ProviderHelpers.GetHashedProvider(Provider); }
        }
    }
}