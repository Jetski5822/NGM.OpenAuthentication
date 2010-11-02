using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public class OpenAuthenticationService : IOpenAuthenticationService {
        private readonly IContentManager _contentManager;
        private readonly IRepository<OpenAuthenticationPartRecord> _openAuthenticationPartRecord;

        public OpenAuthenticationService(IContentManager contentManager, IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRecord) {
            _contentManager = contentManager;
            _openAuthenticationPartRecord = openAuthenticationPartRecord;
        }

        public IUser GetUser(OpenIdIdentifier openIdIdentifier) {
            var record = _openAuthenticationPartRecord.Get(o => o.Identifier == openIdIdentifier.ToString());

            if (record != null)
                return _contentManager.Get<IUser>(record.Id);

            return null;
        }
    }
}