using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace NGM.OpenAuthentication.Models {
    public interface IUserProviders : IContent {
        IList<UserProviderEntry> Providers { get; }
    }

    public class UserProvidersPart : ContentPart, IUserProviders {
        private readonly LazyField<IList<UserProviderEntry>> _providerEntries = new LazyField<IList<UserProviderEntry>>();

        public LazyField<IList<UserProviderEntry>> ProviderEntriesField { get { return _providerEntries; } }

        public IList<UserProviderEntry> Providers {
            get { return ProviderEntriesField.Value; }
            set { ProviderEntriesField.Value = value; }
        }
    }

    public class UserProviderEntry {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string ProviderUserId { get; set; }
    }
}