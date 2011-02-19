using System;
using DotNetOpenAuth.OpenId;

namespace NGM.OpenAuthentication.Core.OpenId {
    public sealed class OpenIdAuthenticationParameters : OpenAuthenticationParameters {
        public OpenIdAuthenticationParameters() {}

        public OpenIdAuthenticationParameters(string externalIdentifier) {
            ExternalIdentifier = externalIdentifier;
        }

        public OpenIdAuthenticationParameters(string externalIdentifier, string friendlyIdentifierForDisplay) : this(externalIdentifier) {
            ExternalDisplayIdentifier = friendlyIdentifierForDisplay;
        }

        public override string Provider {
            get { return "OpenId"; }
        }
    }
}