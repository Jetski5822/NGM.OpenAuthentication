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
            var model = new RegisterModel(_authenticationResponse.ClaimedIdentifier);
            
            MapClaimsToModel(_authenticationResponse.GetExtension<ClaimsResponse>(), model);

            return model;
        }

        private void MapClaimsToModel(ClaimsResponse claimsResponse, RegisterModel registerModel) {
            if (claimsResponse == null)
                return;

            registerModel.Email = claimsResponse.Email;
        }
    }
}