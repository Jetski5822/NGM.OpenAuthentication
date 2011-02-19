using System.Collections.Generic;
using Orchard;

namespace NGM.OpenAuthentication.Core {
    public interface IOpenAuthorizer : IDependency {
        KeyValuePair<string, string> Error { get; }
        OpenAuthenticationStatus Authorize(OpenAuthenticationParameters parameters);
    }
}