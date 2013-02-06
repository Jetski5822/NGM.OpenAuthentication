using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class UserProvidersViewModel {
        public IList<UserProviderEntry> Providers { get; set; }
    }
}