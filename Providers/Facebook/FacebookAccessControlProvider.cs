using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Providers.Facebook {
    public class FacebookAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Facebook"; }
        }
    }
}