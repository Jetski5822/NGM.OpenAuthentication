using System.Collections.Generic;
using System.IdentityModel.Claims;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.CardSpace {
    public class RegisterModelBuilder {
        private readonly IMembershipService _membershipService;

        public RegisterModelBuilder(IMembershipService membershipService) {
            _membershipService = membershipService;
        }

        public RegisterModel Build(string uniqueId, string siteSpecificId, IDictionary<string, string> claims) {
            var parameters = new CardSpaceAuthenticationParameters() {
                ExternalIdentifier = siteSpecificId,
                ExternalDisplayIdentifier = siteSpecificId
            };
            
            //TODO : uniqueId
            var model = new RegisterModel(parameters);

            MapClaimsToModel(claims, model);

            return model;
        }

        private void MapClaimsToModel(IDictionary<string, string> claims, RegisterModel registerModel) {
            if (claims == null)
                return;

            registerModel.UserName = BuildUniqueUserName(claims[ClaimTypes.GivenName] + claims[ClaimTypes.Surname]);
            registerModel.Email = claims[ClaimTypes.Email];
        }

        private string BuildUniqueUserName(string userName) {
            var i = 0;
            while (_membershipService.GetUser(userName) != null) {
                i++;
            }
            return i == 0 ? userName : string.Format("{0}{1}", userName, i);
        }
    }
}