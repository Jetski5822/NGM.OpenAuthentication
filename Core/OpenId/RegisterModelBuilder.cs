using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Core.OpenId {
    public class RegisterModelBuilder {
        private readonly IAuthenticationResponse _authenticationResponse;

        public RegisterModelBuilder(IAuthenticationResponse authenticationResponse) {
            _authenticationResponse = authenticationResponse;
        }

        public RegisterModel Build() {
            var model = new RegisterModel {
                ExternalIdentifier = _authenticationResponse.ClaimedIdentifier,
                ExternalDisplayIdentifier = _authenticationResponse.FriendlyIdentifierForDisplay
            };
            
            MapClaimsToModel(_authenticationResponse.GetExtension<ClaimsResponse>(), model);
            MapFetchClaimsToModel(_authenticationResponse.GetExtension<FetchResponse>(), model);

            return model;
        }

        private static void MapClaimsToModel(ClaimsResponse claimsResponse, RegisterModel registerModel) {
            if (claimsResponse == null)
                return;

            registerModel.Email = claimsResponse.Email;
        }

        private static void MapFetchClaimsToModel(FetchResponse fetchResponse, RegisterModel registerModel) {
            if (fetchResponse == null)
                return;

            registerModel.Email = fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Email);
            registerModel.UserName = fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First) + " " + fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last);
        }
    }
}