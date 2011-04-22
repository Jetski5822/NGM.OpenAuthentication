namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderAuthenticator : IProviderAuthenticator, IOAuthSettings {
        bool IsConsumerConfigured { get; }
        Provider Provider { get; }
    }
}