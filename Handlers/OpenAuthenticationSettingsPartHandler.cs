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

            settings.Record.FacebookClientIdentifier = "152472704810628";
            settings.Record.FacebookClientSecret = "bca66bafbd39c83f60ed85668ab090fc";
            settings.Record.TwitterClientIdentifier = "l7id92UykSoqTBJHhzg";
            settings.Record.TwitterClientSecret = "ZmbZKxvYV0rPblNI0HHhrLX8dU87yTk2EI2dzWf4ImQ";
        }
    }
}