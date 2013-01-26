using JetBrains.Annotations;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace NGM.OpenAuthentication.Handlers {
    [UsedImplicitly]
    public class OpenAuthenticationSettingsPartHandler : ContentHandler {
        public OpenAuthenticationSettingsPartHandler(IRepository<OpenAuthenticationSettingsPartRecord> repository) {
            Filters.Add(new ActivatingFilter<OpenAuthenticationSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}