using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class IndexViewModel {

        public int UserId { get; set; }

        public IList<AccountEntry> Accounts { get; set; }
    }

    public class AccountEntry {
        public OpenAuthenticationPartRecord Account { get; set; }
        public bool IsChecked { get; set; }
    }
}