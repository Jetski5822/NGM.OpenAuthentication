using NGM.OpenAuthentication.Core.Results;
using Orchard;

namespace NGM.OpenAuthentication.Core {
    public interface IAuthenticator : IDependency {
         AuthenticationResult Authenticate(OpenAuthenticationParameters parameters);
    }
}