namespace NGM.OpenAuthentication.Core {
    public sealed class HashedOpenAuthenticationParameters : OpenAuthenticationParameters {
        private readonly IAccessControlProvider _accessControlProvider;

        public HashedOpenAuthenticationParameters(IAccessControlProvider accessControlProvider) {
            _accessControlProvider = accessControlProvider;
        }

        public HashedOpenAuthenticationParameters(IAccessControlProvider hashedProvider, string externalIdentifier)
            : this(hashedProvider) {
            base.ExternalIdentifier = externalIdentifier;
        }

        public override IAccessControlProvider Provider {
            get { return _accessControlProvider; }
        }
    }
}