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
        private readonly IRepository<OpenAuthenticationSettingsPartRecord> _openAuthenticationSettingsRecordRepository;
        private readonly IMembershipService _membershipService;
        private readonly IOrchardServices _orchardServices;

        public OpenAuthenticationService(IContentManager contentManager, 
            IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRecordRespository, 
            IRepository<OpenAuthenticationSettingsPartRecord> openAuthenticationSettingsRecordRepository,
            IMembershipService membershipService,
            IOrchardServices orchardServices) {

            _contentManager = contentManager;
            _openAuthenticationPartRecordRespository = openAuthenticationPartRecordRespository;
            _openAuthenticationSettingsRecordRepository = openAuthenticationSettingsRecordRepository;
            _membershipService = membershipService;
            _orchardServices = orchardServices;
        }

        public void AssociateOpenIdWithUser(IUser user, string openIdIdentifier, string friendlyOpenIdIdentifier) {
            var openAuthenticationPart= _orchardServices.ContentManager.Create<OpenAuthenticationPart>("User");
            openAuthenticationPart.Record.UserId = user.Id;
            openAuthenticationPart.Record.ClaimedIdentifier = openIdIdentifier;
            openAuthenticationPart.Record.FriendlyIdentifierForDisplay = friendlyOpenIdIdentifier;
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
                return _contentManager.Get<IUser>(record.UserId);
            }

            return null;
        }

        public OpenAuthenticationSettingsPart GetSettings() {
            return _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>();
        }

        public IContentQuery<OpenAuthenticationPart, OpenAuthenticationPartRecord> GetIdentifiersFor(IUser user) {
            return _contentManager
               .Query<OpenAuthenticationPart, OpenAuthenticationPartRecord>()
               .Where(c => c.UserId == user.Id);
        }

        public void RemoveOpenIdAssociation(string openIdIdentifier) {
            var record = _openAuthenticationPartRecordRespository.Get(o => o.ClaimedIdentifier == openIdIdentifier);

            if (record != null)
                _openAuthenticationPartRecordRespository.Delete(record);
        }
    }
}