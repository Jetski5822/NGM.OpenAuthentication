using System.Collections.Generic;
using NGM.OpenAuthentication.Core.Claims;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class OAuthAuthenticationParameters : OpenAuthenticationParameters {
        private readonly IAccessControlProvider _provider;
        private IList<UserClaims> _claims;

        public OAuthAuthenticationParameters(IAccessControlProvider provider) {
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

        public override IAccessControlProvider Provider {
            get { return _provider; }
        }
    }
}