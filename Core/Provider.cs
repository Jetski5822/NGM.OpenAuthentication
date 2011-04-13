namespace NGM.OpenAuthentication.Core {
    public enum Provider {
        OpenId,
        Twitter,
        Facebook,
        LiveId,
        CardSpace
    }

    public static class ProviderHelpers {
        public static int GetHashedProvider(Provider provider) {
            return provider.ToString().GetHashCode();
        }
    }
}