using NGM.OpenAuthentication.Core;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Facebook.Services {
    [OrchardFeature("Authentication.Facebook")]
    public class FacebookAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Facebook"; }
        }
    }
}