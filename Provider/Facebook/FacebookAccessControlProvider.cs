using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Facebook {
    public class FacebookAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Facebook"; }
        }
    }
}