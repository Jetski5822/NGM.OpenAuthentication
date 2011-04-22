using Orchard;

namespace NGM.OpenAuthentication.Core {
    public interface IProviderAuthenticator : IDependency {
        AuthenticationState Authenticate(string returnUrl);
    }
}