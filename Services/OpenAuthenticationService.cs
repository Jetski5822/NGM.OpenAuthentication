using System;
using System.Collections.Generic;
using System.Linq;
using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public class OpenAuthenticationService : IOpenAuthenticationService {
        private readonly IContentManager _contentManager;
        private readonly IRepository<OpenAuthenticationPartRecord> _openAuthenticationPartRecordRespository;
        private readonly IRepository<OpenAuthenticationSettingsRecord> _openAuthenticationSettingsRecordRepository;
        private readonly IMembershipService _membershipService;

        public OpenAuthenticationService(IContentManager contentManager, 
            IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRecordRespository, 
            IRepository<OpenAuthenticationSettingsRecord> openAuthenticationSettingsRecordRepository,
            IMembershipService membershipService) {

            _contentManager = contentManager;
            _openAuthenticationPartRecordRespository = openAuthenticationPartRecordRespository;
            _openAuthenticationSettingsRecordRepository = openAuthenticationSettingsRecordRepository;
            _membershipService = membershipService;
        }

        public void AssociateOpenIdWithUser(IUser user, string openIdIdentifier, string friendlyOpenIdIdentifier) {
            var account = user.As<OpenAuthenticationPart>();
            account.Record.ClaimedIdentifier = openIdIdentifier;
            account.Record.FriendlyIdentifierForDisplay = friendlyOpenIdIdentifier;
            _openAuthenticationPartRecordRespository.Create(account.Record);
        }

        public bool IsAccountExists(string identifier) {
            return GetUser(identifier) != null;
        }

        public IUser CreateUser(RegisterModel registerModel) {
            // Randomise the password as we dont care at this point.
            var user = _membershipService.CreateUser(
                new CreateUserParams(
                    registerModel.ClaimedIdentifier, new byte[8].ToString(), registerModel.Email, null, null, true));

            // Okay, now we have the user, lets associate the open id with the account
            AssociateOpenIdWithUser(user, registerModel.ClaimedIdentifier, registerModel.FriendlyIdentifier);

            return user;
        }

        public IUser GetUser(string openIdIdentifier) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ClaimedIdentifier == openIdIdentifier);

            if (record != null) {
                return _contentManager.Get<IUser>(record.Id);
            }

            return null;
        }

        public OpenAuthenticationSettingsRecord GetSettings() {
            var settings = from openSettings in _openAuthenticationSettingsRecordRepository.Table select openSettings;
            return settings.FirstOrDefault<OpenAuthenticationSettingsRecord>();
        }

        public IContentQuery<OpenAuthenticationPart, OpenAuthenticationPartRecord> GetIdentifiersFor(IUser user) {
            return _contentManager
               .Query<OpenAuthenticationPart, OpenAuthenticationPartRecord>()
               .Where(c => c.Id == user.Id);
        }

        public void RemoveOpenIdAssociation(string openIdIdentifier) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ClaimedIdentifier == openIdIdentifier);

            if (record != null)
                _openAuthenticationPartRecordRespository.Delete(record);
        }
    }
}