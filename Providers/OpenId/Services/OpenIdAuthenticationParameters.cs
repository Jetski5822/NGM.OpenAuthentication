using System.Collections.Generic;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.Claims;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.OpenId.Services {
    [OrchardFeature("OpenId")]
    public sealed class OpenIdAuthenticationParameters : OpenAuthenticationParameters {
        private readonly IList<UserClaims> _claims;

        public OpenIdAuthenticationParameters() {}

        public OpenIdAuthenticationParameters(IAuthenticationResponse authenticationResponse) {
            ExternalIdentifier = authenticationResponse.ClaimedIdentifier;
            ExternalDisplayIdentifier = authenticationResponse.FriendlyIdentifierForDisplay;

            _claims = new List<UserClaims>();
            var claimsResponseTranslator = new OpenIdClaimsResponseClaimsTranslator();
            var claims1 = claimsResponseTranslator.Translate(authenticationResponse.GetExtension<ClaimsResponse>());
            if (claims1 != null)
                UserClaims.Add(claims1);

            var fetchResponseTranslator = new OpenIdFetchResponseClaimsTranslator();
            var claims2 = fetchResponseTranslator.Translate(authenticationResponse.GetExtension<FetchResponse>());
            if (claims2 != null)
                UserClaims.Add(claims2);
        }

        public override IList<UserClaims> UserClaims {
            get {
                return _claims;
            }
        }

        public override IAccessControlProvider Provider {
            get { return new OpenIdAccessControlProvider(); }
        }
    }
}