using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class AdminIndexViewModel {
        public IList<AccountEntry> Accounts { get; set; }
        public AdminIndexOptions Options { get; set; }
    }

    public class AccountEntry {
        public OpenAuthenticationPartRecord Account { get; set; }
        public bool IsChecked { get; set; }
    }

    public class AdminIndexOptions {
        public AdminBulkAction BulkAction { get; set; }
    }

    public enum AdminBulkAction {
        None,
        Delete
    }
}