namespace NGM.OpenAuthentication.Core {
    public abstract class OpenAuthenticationParameters {
        public abstract string Provider { get; }
        public string ExternalIdentifier { get; set; }
        public string ExternalDisplayIdentifier { get; set; }
        public string OAuthToken { get; set; }
        public string OAuthAccessToken { get; set; }

        public virtual int HashedProvider {
            get { return Provider.GetHashCode(); }
        }
    }
}