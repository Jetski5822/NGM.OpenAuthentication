using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class IndexViewModel {
        public bool AutoRegistrationEnabled { get; set; }

        public IEnumerable<ProviderConfigurationRecord> CurrentProviders { get; set; }
    }
}