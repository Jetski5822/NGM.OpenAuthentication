using System.Collections.Generic;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class VerifiedAccountsViewModel {
        public VerifiedAccountsViewModel() {}

        public VerifiedAccountsViewModel(IEnumerable<AccountModel> accountModels) {
            AccountModels = accountModels;
        }

        public IEnumerable<AccountModel> AccountModels { get; set;}
    }
}