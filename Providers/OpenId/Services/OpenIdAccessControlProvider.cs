using NGM.OpenAuthentication.Core;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.OpenId.Services {
    [OrchardFeature("OpenId")]
    public class OpenIdAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "OpenId"; }
        }
    }
}