using DotNetOpenAuth.OpenId.RelyingParty;

namespace NGM.OpenAuthentication.Core.OpenId {
    public interface IOpenIdRelyingPartyService {
        IAuthenticationResponse Response { get; }
        IAuthenticationRequest CreateRequest(OpenIdIdentifier openIdIdentifier);
        bool HasResponse { get; }
    }
}