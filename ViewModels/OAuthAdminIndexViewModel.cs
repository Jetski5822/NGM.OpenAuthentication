using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class OAuthAdminIndexViewModel {
        public IList<OAuthProviderSettingsPart> Providers { get; set; }
    }
}