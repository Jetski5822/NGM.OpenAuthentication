using JetBrains.Annotations;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace NGM.OpenAuthentication.Handlers {
    [UsedImplicitly]
    public class OpenAuthenticationSettingsPartHandler : ContentHandler {
        public OpenAuthenticationSettingsPartHandler(IRepository<OpenAuthenticationSettingsPartRecord> repository) {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<OpenAuthenticationSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
            OnActivated<OpenAuthenticationSettingsPart>(DefaultSettings);
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("OpenAuthentication")));
        }

        private static void DefaultSettings(ActivatedContentContext context, OpenAuthenticationSettingsPart settings) {
            settings.Record.Email = true;
            settings.Record.FullName = true;

            settings.Record.FacebookClientIdentifier = "137176593016708";
            settings.Record.FacebookClientSecret = "c7d138e1058fd15224af664bd1ccea5b";
            settings.Record.TwitterClientIdentifier = "l7id92UykSoqTBJHhzg";
            settings.Record.TwitterClientSecret = "ZmbZKxvYV0rPblNI0HHhrLX8dU87yTk2EI2dzWf4ImQ";
            settings.Record.LiveIdClientIdentifier = "0000000044045D09";
            settings.Record.LiveIdClientSecret = "8QT2XdAUHpI7Wh6tNpGFZI6DFdOJKt2r";
        }
    }
}