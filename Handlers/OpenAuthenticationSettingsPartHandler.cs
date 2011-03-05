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

            settings.Record.FacebookClientIdentifier = "137176593016708";
            settings.Record.FacebookClientSecret = "c7d138e1058fd15224af664bd1ccea5b";
            settings.Record.TwitterClientIdentifier = "l7id92UykSoqTBJHhzg";
            settings.Record.TwitterClientSecret = "ZmbZKxvYV0rPblNI0HHhrLX8dU87yTk2EI2dzWf4ImQ";
            settings.Record.LiveIdClientIdentifier = "0000000044045D09";
            settings.Record.LiveIdClientSecret = "8QT2XdAUHpI7Wh6tNpGFZI6DFdOJKt2r";
        }
    }
}