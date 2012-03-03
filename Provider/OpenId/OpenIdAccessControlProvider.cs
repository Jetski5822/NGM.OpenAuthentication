using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Provider.OpenId {
    public class OpenIdAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "OpenId"; }
        }
    }
}