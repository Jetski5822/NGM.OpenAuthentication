using System;
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

        //private readonly IContentManager _contentManager;
        //private readonly IRepository<OpenAuthenticationPartRecord> _openAuthenticationPartRecord;
        //private readonly IExtendedPropertiesEventHandler _extendedPropertiesEventHandler;

        //public OpenAuthenticationService(IContentManager contentManager,
        //    IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRecord,
        //    IExtendedPropertiesEventHandler extendedPropertiesEventHandler) {
        //    _contentManager = contentManager;
        //    _openAuthenticationPartRecord = openAuthenticationPartRecord;
        //    _extendedPropertiesEventHandler = extendedPropertiesEventHandler;
        //}

        //public IUser GetUserFor(OpenIdIdentifier openIdIdentifier) {
        //    var record = Get(openIdIdentifier);

        //    if (record != null)
        //        return _contentManager.Get<IUser>(record.Id);

        //    return null;
        //}

        //public void AssociateOpenIdWithUser(IUser user, OpenIdIdentifier openIdIdentifier) {
        //    var existingUser = GetUserFor(openIdIdentifier);

        //    if (existingUser.Equals(user))
        //        UpdateExistingRecord(Get(openIdIdentifier), openIdIdentifier);
        //    else {
        //        CreateRecord(user, openIdIdentifier);
        //    }
        //}

        //private void CreateRecord(IUser user, OpenIdIdentifier openIdIdentifier) {
        //    var part = user.As<OpenAuthenticationPart>();
        //    part.Identifier = openIdIdentifier.ToString();
        //}

        //private void UpdateExistingRecord(OpenAuthenticationPartRecord openAuthenticationPartRecord, OpenIdIdentifier openIdIdentifier) {
        //    openAuthenticationPartRecord.Identifier = openIdIdentifier.ToString();
        //}

        //private OpenAuthenticationPartRecord Get(OpenIdIdentifier openIdIdentifier) {
        //    return _openAuthenticationPartRecord.Get(o => o.Identifier == openIdIdentifier.ToString());
        //}

    }
}