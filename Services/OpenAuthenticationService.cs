using System;
using System.Collections.Generic;
using System.Linq;
using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public class OpenAuthenticationService : IOpenAuthenticationService {
        private readonly IContentManager _contentManager;
        private readonly IRepository<OpenAuthenticationPartRecord> _openAuthenticationPartRecordRespository;
        private readonly IOrchardServices _orchardServices;

        public OpenAuthenticationService(IContentManager contentManager, 
            IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRecordRespository,
            IOrchardServices orchardServices) {

            _contentManager = contentManager;
            _openAuthenticationPartRecordRespository = openAuthenticationPartRecordRespository;
            _orchardServices = orchardServices;
        }

        public void AssociateExternalAccountWithUser(IUser user, string externalIdentifier, string externalDisplayIdentifier) {
            _orchardServices.ContentManager.Create<OpenAuthenticationPart>("User",
                                                                           (o) => {
                                                                               o.Record.UserId = user.Id;
                                                                               o.Record.ExternalIdentifier = externalIdentifier;
                                                                               o.Record.ExternalDisplayIdentifier = externalDisplayIdentifier;
                                                                           });
        }

        public bool AccountExists(string externalIdentifier) {
            return GetUser(externalIdentifier) != null;
        }

        public IUser GetUser(string externalIdentifier) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ExternalIdentifier == externalIdentifier);

            if (record != null) {
                return _contentManager.Get<IUser>(record.UserId);
            }

            return null;
        }

        public OpenAuthenticationSettingsPart GetSettings() {
            return _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>();
        }

        public IContentQuery<OpenAuthenticationPart, OpenAuthenticationPartRecord> GetExternalIdentifiersFor(IUser user) {
            return _contentManager
               .Query<OpenAuthenticationPart, OpenAuthenticationPartRecord>()
               .Where(c => c.UserId == user.Id);
        }

        public void RemoveOpenIdAssociation(string externalIdentifier) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ExternalIdentifier == externalIdentifier);

            if (record != null)
                _openAuthenticationPartRecordRespository.Delete(record);
        }
    }
}