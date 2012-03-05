using NGM.OpenAuthentication.Core;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Twitter.Services {
    [OrchardFeature("Twitter")]
    public class TwitterAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Twitter"; }
        }
    }
}