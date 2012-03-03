namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderAuthenticator : IProviderAuthenticator, IOAuthSettings {
        bool IsConsumerConfigured { get; }
        AccessControlProvider Provider { get; }
    }
}