using JetBrains.Annotations;
using NGM.OpenAuthentication.Core.OAuth;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace NGM.OpenAuthentication.Handlers {
    [UsedImplicitly]
    public class OAuthProviderSettingsPartHandler : ContentHandler {
        private readonly IRepository<OAuthProviderSettingsPartRecord> _repository;

        public OAuthProviderSettingsPartHandler(IRepository<OAuthProviderSettingsPartRecord> repository) {
            _repository = repository;
            Filters.Add(new ActivatingFilter<OAuthProviderSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));

            SetupDefaultSettings();
        }

        private void SetupDefaultSettings() {
            var facebookPart = new OAuthProviderSettingsPart();
            facebookPart.Record.Provider = OAuthProvider.Facebook.ToString();
            facebookPart.Record.ClientIdentifier = "137176593016708";
            facebookPart.Record.ClientSecret = "c7d138e1058fd15224af664bd1ccea5b";

            var twitterPart = new OAuthProviderSettingsPart();
            facebookPart.Record.Provider = OAuthProvider.Twitter.ToString();
            facebookPart.Record.ClientIdentifier = "l7id92UykSoqTBJHhzg";
            facebookPart.Record.ClientSecret = "ZmbZKxvYV0rPblNI0HHhrLX8dU87yTk2EI2dzWf4ImQ";

            _repository.Create(facebookPart.Record);
            _repository.Create(twitterPart.Record);
        }
    }
}