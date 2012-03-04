using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Providers.Twitter {
    public class TwitterAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Twitter"; }
        }
    }
}