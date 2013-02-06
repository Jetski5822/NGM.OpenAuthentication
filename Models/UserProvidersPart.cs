using System.Collections.Generic;
using Orchard.ContentManagement;

namespace NGM.OpenAuthentication.Models {
    public interface IUserProviders : IContent {
        IList<UserProviderEntry> Providers { get; }
    }

    public class UserProvidersPart : ContentPart, IUserProviders {
        public UserProvidersPart() {
            Providers = new List<UserProviderEntry>();
        }

        public IList<UserProviderEntry> Providers { get; set; }
    }

    public class UserProviderEntry {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string ProviderUserId { get; set; }
    }
}