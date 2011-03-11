using Orchard;

namespace NGM.OpenAuthentication.Core {
    public interface IAuthorizer : IDependency {
         AuthorizationResult Authorize(OpenAuthenticationParameters parameters);
    }
}