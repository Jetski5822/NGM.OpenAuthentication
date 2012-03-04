using NGM.OpenAuthentication.Core;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Facebook {
    [OrchardFeature("Facebook")]
    public class FacebookAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Facebook"; }
        }
    }
}