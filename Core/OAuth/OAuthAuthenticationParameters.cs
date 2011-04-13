using System.Collections.Generic;
using NGM.OpenAuthentication.Core.Claims;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class OAuthAuthenticationParameters : OpenAuthenticationParameters {
        private readonly Provider _provider;
        private IList<UserClaims> _claims;

        public OAuthAuthenticationParameters(Provider provider) {
            _provider = provider;
        }

        public override IList<UserClaims> UserClaims {
            get {
                return _claims;
            }
        }

        public void AddClaim(UserClaims claim) {
            if (_claims == null)
                _claims = new List<UserClaims>();

            _claims.Add(claim);
        }

        public override Provider Provider {
            get { return _provider; }
        }
    }
}