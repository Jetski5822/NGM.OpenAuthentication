using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Provider.MicrosoftConnect {
    public class MicrosoftConnectAccessControlProvider : AccessControlProvider {
        public override string Name {
            get { return "Microsoft Connect"; }
        }
    }
}