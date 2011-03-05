using System.Linq;
using NGM.OpenAuthentication.Core.OAuth;
using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.ContentManagement;

namespace NGM.OpenAuthentication.Services {
    public class OAuthProviderServices : IOAuthProviderServices {
        private readonly IOrchardServices _orchardServices;

        public OAuthProviderServices(IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
        }

        public OAuthProviderSettingsPart GetProviderSettings(OAuthProvider provider) {
            return _orchardServices.ContentManager
                .Query<OAuthProviderSettingsPart, OAuthProviderSettingsPartRecord>()
                .Where(c => c.Provider == provider.ToString()).List().FirstOrDefault<OAuthProviderSettingsPart>();
        }
    }
}