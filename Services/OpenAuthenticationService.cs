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

        public IUser GetUserFor(OpenIdIdentifier openIdIdentifier) {
            var record = Get(openIdIdentifier);

            if (record != null)
                return _contentManager.Get<IUser>(record.Id);

            return null;
        }

        public AssociateOpenIdWithUser(IUser user, OpenIdIdentifier openIdIdentifier) {
            var existingUser = GetUserFor(openIdIdentifier);

            if (existingUser.Equals(user))
                UpdateExistingRecord(Get(openIdIdentifier), openIdIdentifier);
            else {
                CreateRecord(user, openIdIdentifier);
            }
        }

        private void CreateRecord(IUser user, OpenIdIdentifier openIdIdentifier) {
            var part = user.As<OpenAuthenticationPart>();
            part.Identifier = openIdIdentifier.ToString();
        }

        private void UpdateExistingRecord(OpenAuthenticationPartRecord openAuthenticationPartRecord, OpenIdIdentifier openIdIdentifier) {
            openAuthenticationPartRecord.Identifier = openIdIdentifier.ToString();
        }

        private OpenAuthenticationPartRecord Get(OpenIdIdentifier openIdIdentifier) {
            return _openAuthenticationPartRecord.Get(o => o.Identifier == openIdIdentifier.ToString());
        }
    }
}