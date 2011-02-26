namespace NGM.OpenAuthentication.Core.OpenId {
    public interface IOpenIdProviderAuthorizer : IProviderAuthorizer {
        bool IsOpenIdCallback { get; }
    }
}