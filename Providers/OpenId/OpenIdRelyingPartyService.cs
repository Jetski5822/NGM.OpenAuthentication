using DotNetOpenAuth.OpenId.RelyingParty;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.OpenId {
    [OrchardFeature("OpenId")]
    public class OpenIdRelyingPartyService : IOpenIdRelyingPartyService {
        private static readonly OpenIdRelyingParty _relyingParty = new OpenIdRelyingParty();

        private IAuthenticationResponse _response;

        public IAuthenticationResponse Response {
            get {
                if (_response == null) {
                    _response = _relyingParty.GetResponse();
                }
                return _response;
            }
        }

        public IAuthenticationRequest CreateRequest(OpenIdIdentifier openIdIdentifier) {
            return _relyingParty.CreateRequest(openIdIdentifier.Identifier);
        }

        public bool HasResponse {
            get {
                return Response != null;
            }
        }
    }
}