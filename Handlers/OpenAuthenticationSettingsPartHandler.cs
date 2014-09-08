using JetBrains.Annotations;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace NGM.OpenAuthentication.Handlers {
    [UsedImplicitly]
    public class OpenAuthenticationSettingsPartHandler : ContentHandler {
        public OpenAuthenticationSettingsPartHandler() {
            Filters.Add(new ActivatingFilter<OpenAuthenticationSettingsPart>("Site"));
        }
    }
}