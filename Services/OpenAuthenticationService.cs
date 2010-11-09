using System;
using System.Linq;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public class OpenAuthenticationService : IOpenAuthenticationService {
        private readonly IContentManager _contentManager;
        private readonly IRepository<OpenAuthenticationPartRecord> _openAuthenticationPartRecord;
        private readonly IRepository<OpenAuthenticationSettingsRecord> _openAuthenticationSettingsRecordRepository;

        public OpenAuthenticationService(IContentManager contentManager, IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRecord, IRepository<OpenAuthenticationSettingsRecord> openAuthenticationSettingsRecordRepository) {
            _contentManager = contentManager;
            _openAuthenticationPartRecord = openAuthenticationPartRecord;
            _openAuthenticationSettingsRecordRepository = openAuthenticationSettingsRecordRepository;
        }

        public void AssociateOpenIdWithUser(IUser user, string openIdIdentifier) {
            throw new NotImplementedException();
        }

        public IUser CreateUser(string openIdIdentifier) {
            throw new NotImplementedException();
        }

        public IUser GetUser(string openIdIdentifier) {
            var record = _openAuthenticationPartRecord.Get(o => o.Identifier == openIdIdentifier);

            if (record != null) {
                return _contentManager.Get<IUser>(record.Id);
            }

            return null;
        }

        public OpenAuthenticationSettingsRecord GetSettings() {
            var settings = from openSettings in _openAuthenticationSettingsRecordRepository.Table select openSettings;
            return settings.FirstOrDefault<OpenAuthenticationSettingsRecord>();
        }
    }
}