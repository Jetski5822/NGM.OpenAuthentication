namespace NGM.OpenAuthentication.Core.OpenId {
    public interface IOpenIdProviderAuthorizer : IProviderAuthorizer {
        string EnternalIdentifier { get; set; } // mayne - refactor this out
        bool IsOpenIdCallback { get; }
    }
}