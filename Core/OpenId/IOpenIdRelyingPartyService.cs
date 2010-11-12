using DotNetOpenAuth.OpenId.RelyingParty;
using Orchard;

namespace NGM.OpenAuthentication.Core.OpenId {
    public interface IOpenIdRelyingPartyService : IDependency {
        IAuthenticationResponse Response { get; }
        IAuthenticationRequest CreateRequest(OpenIdIdentifier openIdIdentifier);
        bool HasResponse { get; }
    }
}