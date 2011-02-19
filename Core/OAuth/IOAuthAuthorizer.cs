using Orchard;

namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthAuthorizer : IOAuthSettings, IDependency {
        bool IsConsumerConfigured { get; }
        AuthorizeState Authorize(string returnUrl);
        string Provider { get; }
    }
}