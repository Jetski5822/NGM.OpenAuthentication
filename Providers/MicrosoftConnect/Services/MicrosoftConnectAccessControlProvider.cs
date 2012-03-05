using NGM.OpenAuthentication.Core;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.MicrosoftConnect.Services {
    [OrchardFeature("MicrosoftConnect")]
    public class MicrosoftConnectAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Microsoft Connect"; }
        }
    }
}