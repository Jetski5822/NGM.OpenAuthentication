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
            Filters.Add(new TemplateFilterForRecord<OpenAuthenticationSettingsPartRecord>("OpenAuthenticationSettings", "Parts/OpenAuthentication.Settings"));
            OnActivated<OpenAuthenticationSettingsPart>(DefaultSettings);
        }

        private static void DefaultSettings(ActivatedContentContext context, OpenAuthenticationSettingsPart settings) {
            settings.Record.Email = true;
            settings.Record.FullName = true;

            settings.Record.OpenIdEnabled = true;
            settings.Record.CardSpaceEnabled = false;
            settings.Record.OAuthEnabled = true;
        }
    }
}