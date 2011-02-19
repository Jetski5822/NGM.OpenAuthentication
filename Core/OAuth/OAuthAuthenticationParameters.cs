namespace NGM.OpenAuthentication.Core.OAuth {
    public class OAuthAuthenticationParameters : OpenAuthenticationParameters {
        private readonly OAuthProvider _provider;

        public OAuthAuthenticationParameters(OAuthProvider provider) {
            _provider = provider;
        }

        public override string Provider {
            get { return _provider.ToString(); }
        }
    }
}