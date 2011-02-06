using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Models;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OpenId {
    public class RegisterModelBuilder {
        private readonly IAuthenticationResponse _authenticationResponse;
        private readonly IMembershipService _membershipService;

        public RegisterModelBuilder(IAuthenticationResponse authenticationResponse, IMembershipService membershipService) {
            _authenticationResponse = authenticationResponse;
            _membershipService = membershipService;
        }

        public RegisterModel Build() {
            var model = new RegisterModel {
                ClaimedIdentifier = _authenticationResponse.ClaimedIdentifier,
                FriendlyIdentifier = _authenticationResponse.FriendlyIdentifierForDisplay
            };
            
            MapClaimsToModel(_authenticationResponse.GetExtension<ClaimsResponse>(), model);
            MapFetchClaimsToModel(_authenticationResponse.GetExtension<FetchResponse>(), model);

            return model;
        }

        private void MapClaimsToModel(ClaimsResponse claimsResponse, RegisterModel registerModel) {
            if (claimsResponse == null)
                return;

            registerModel.Email = claimsResponse.Email;
        }

        private void MapFetchClaimsToModel(FetchResponse fetchResponse, RegisterModel registerModel) {
            if (fetchResponse == null)
                return;

            registerModel.Email = fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Email);
            registerModel.UserName = BuildUniqueUserName(fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First) + fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last));
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