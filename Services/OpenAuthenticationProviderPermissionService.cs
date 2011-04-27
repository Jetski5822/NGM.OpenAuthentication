using System.Linq;
using System.Collections.Generic;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;

namespace NGM.OpenAuthentication.Services {
    public class OpenAuthenticationProviderPermissionService : IOpenAuthenticationProviderPermissionService {
        private readonly IContentManager _contentManager;

        public OpenAuthenticationProviderPermissionService(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public bool IsPermissionEnabled(string scope, Provider provider) {
            var value = Get(provider).FirstOrDefault(o => o.Record.Scope == scope);
            return value != null && value.Record.IsEnabled;
        }

        public IEnumerable<OpenAuthenticationPermissionSettingsPart> Get(Provider provider) {
            var hashedProvider = ProviderHelpers.GetHashedProvider(provider);
            return GetAll().Where(o => o.Record.HashedProvider == hashedProvider);
        }

        public IEnumerable<OpenAuthenticationPermissionSettingsPart> GetAll() {
            return _contentManager
                .Query<OpenAuthenticationPermissionSettingsPart, OpenAuthenticationPermissionSettingsPartRecord>()
                .List();
        }
    }
}