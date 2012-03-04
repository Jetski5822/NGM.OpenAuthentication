using NGM.OpenAuthentication.Core;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Twitter {
    [OrchardFeature("Twitter")]
    public class TwitterAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Twitter"; }
        }
    }
}