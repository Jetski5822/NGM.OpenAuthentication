using System;

namespace NGM.OpenAuthentication.Core {
    public sealed class HashedOpenAuthenticationParameters : OpenAuthenticationParameters {
        private readonly string _hashedProvider;

        public HashedOpenAuthenticationParameters(string hashedProvider) {
            _hashedProvider = hashedProvider;
        }

        public HashedOpenAuthenticationParameters(string hashedProvider, string externalIdentifier) : this(hashedProvider) {
            base.ExternalIdentifier = externalIdentifier;
        }

        public override Provider Provider {
            get { throw new NotSupportedException();}
        }

        public override string HashedProvider {
            get { return _hashedProvider; }
        }
    }
}