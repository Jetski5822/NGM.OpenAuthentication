namespace NGM.OpenAuthentication.Core.OpenId {
    public interface IOpenIdProviderAuthenticator : IProviderAuthenticator {
        string EnternalIdentifier { get; set; } // mayne - refactor this out
        bool IsOpenIdCallback { get; }
    }
}