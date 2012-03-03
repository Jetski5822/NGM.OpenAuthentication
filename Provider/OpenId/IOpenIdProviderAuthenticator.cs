using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Provider.OpenId {
    public interface IOpenIdProviderAuthenticator : IProviderAuthenticator {
        string EnternalIdentifier { get; set; } // mayne - refactor this out
        bool IsOpenIdCallback { get; }
    }
}