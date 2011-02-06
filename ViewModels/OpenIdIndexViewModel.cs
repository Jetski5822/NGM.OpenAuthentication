using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class OpenIdIndexViewModel {
        public IList<AccountEntry> Accounts { get; set; }
        public OpenIdIndexOptions Options { get; set; }
    }

    public class AccountEntry {
        public OpenAuthenticationPartRecord Account { get; set; }
        public bool IsChecked { get; set; }
    }

    public class OpenIdIndexOptions {
        public OpenIdBulkAction BulkAction { get; set; }
    }

    public enum OpenIdBulkAction {
        None,
        Delete
    }
}