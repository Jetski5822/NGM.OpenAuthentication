namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthSettings {
        string ClientKeyIdentifier { get; }
        string ClientSecret { get; }
    }
}