namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderAuthorizer : IProviderAuthorizer, IOAuthSettings {
        bool IsConsumerConfigured { get; }
        Provider Provider { get; }
    }
}