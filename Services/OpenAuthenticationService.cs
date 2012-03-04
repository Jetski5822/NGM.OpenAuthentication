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
            var part = _orchardServices.ContentManager.Create<OpenAuthenticationPart>("OpenAuthentication",
                                                                           o => {
                                                                               o.UserId = user.Id;
                                                                               o.ExternalIdentifier = parameters.ExternalIdentifier;
                                                                               o.ExternalDisplayIdentifier = parameters.ExternalDisplayIdentifier;
                                                                               o.OAuthToken = parameters.OAuthToken;
                                                                               o.OAuthAccessToken = parameters.OAuthAccessToken;
                                                                               o.HashedProvider = parameters.Provider.Hash;
                                                                           });
            part.ContentItem.ContentManager.Flush();
        }

        public bool AccountExists(OpenAuthenticationParameters parameters) {
            return GetUser(parameters) != null;
        }

        public IUser GetUser(OpenAuthenticationParameters parameters) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ExternalIdentifier == parameters.ExternalIdentifier && o.HashedProvider == parameters.Provider.Hash);

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

        public void RemoveAssociation(OpenAuthenticationParameters parameters) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ExternalIdentifier == parameters.ExternalIdentifier && o.HashedProvider == parameters.Provider.Hash);

            if (record != null)
                _openAuthenticationPartRecordRespository.Delete(record);
        }
    }
}