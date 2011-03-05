using NGM.OpenAuthentication.Core.OAuth;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Services {
    public interface IOAuthProviderServices {
        OAuthProviderSettingsPart GetProviderSettings(OAuthProvider provider);
    }
}