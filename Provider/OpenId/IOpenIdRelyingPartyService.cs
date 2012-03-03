using DotNetOpenAuth.OpenId.RelyingParty;
using Orchard;

namespace NGM.OpenAuthentication.Provider.OpenId {
    public interface IOpenIdRelyingPartyService : IDependency {
        IAuthenticationResponse Response { get; }
        IAuthenticationRequest CreateRequest(OpenIdIdentifier openIdIdentifier);
        bool HasResponse { get; }
    }
}