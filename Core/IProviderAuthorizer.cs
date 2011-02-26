using Orchard;

namespace NGM.OpenAuthentication.Core {
    public interface IProviderAuthorizer : IDependency {
        AuthorizeState Authorize(string returnUrl);
    }
}