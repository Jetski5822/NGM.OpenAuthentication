using DotNetOpenAuth.OpenId.RelyingParty;

namespace NGM.OpenAuthentication.Core {
    public class OpenIdRelyingPartyWrapper {
        protected OpenIdRelyingParty _relyingParty;

        private IAuthenticationResponse _response;

        public OpenIdRelyingPartyWrapper() {
            _relyingParty = new OpenIdRelyingParty();
        }

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