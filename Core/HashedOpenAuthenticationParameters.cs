using System;

namespace NGM.OpenAuthentication.Core {
    public sealed class HashedOpenAuthenticationParameters : OpenAuthenticationParameters {
        private readonly int _hashedProvider;
        
        public HashedOpenAuthenticationParameters(int hashedProvider) {
            _hashedProvider = hashedProvider;
        }

        public HashedOpenAuthenticationParameters(int hashedProvider, string externalIdentifier) : this(hashedProvider) {
            base.ExternalIdentifier = externalIdentifier;
        }

        public override string Provider {
            get { throw new NotSupportedException();}
        }

        public override int HashedProvider {
            get { return _hashedProvider; }
        }
    }
}