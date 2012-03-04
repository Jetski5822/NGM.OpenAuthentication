using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Providers.OpenId {
    public class OpenIdAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "OpenId"; }
        }
    }
}