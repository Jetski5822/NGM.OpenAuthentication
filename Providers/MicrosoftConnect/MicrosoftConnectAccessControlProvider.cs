using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Providers.MicrosoftConnect {
    public class MicrosoftConnectAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Microsoft Connect"; }
        }
    }
}