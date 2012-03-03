using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Provider.Facebook {
    public class FacebookAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Facebook"; }
        }
    }
}