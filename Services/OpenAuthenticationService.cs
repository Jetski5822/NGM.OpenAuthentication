using NGM.OpenAuthentication.Core;
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

        public void AssociateExternalAccountWithUser(IUser user, OpenAuthenticationParameters parameters) {
            _orchardServices.ContentManager.Create<OpenAuthenticationPart>("User",
                                                                           (o) => {
                                                                               o.Record.UserId = user.Id;
                                                                               o.Record.ExternalIdentifier = parameters.ExternalIdentifier;
                                                                               o.Record.ExternalDisplayIdentifier = parameters.ExternalDisplayIdentifier;
                                                                               o.Record.OAuthToken = parameters.OAuthToken;
                                                                               o.Record.OAuthAccessToken = parameters.OAuthAccessToken;
                                                                               o.Record.HashedProvider = parameters.HashedProvider;
                                                                           });
        }

        public bool AccountExists(OpenAuthenticationParameters parameters) {
            return GetUser(parameters) != null;
        }

        public IUser GetUser(OpenAuthenticationParameters parameters) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ExternalIdentifier == parameters.ExternalIdentifier && o.HashedProvider == parameters.HashedProvider);

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

        public void RemoveOpenIdAssociation(OpenAuthenticationParameters parameters) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ExternalIdentifier == parameters.ExternalIdentifier && o.HashedProvider == parameters.HashedProvider);

            if (record != null)
                _openAuthenticationPartRecordRespository.Delete(record);
        }
    }
}