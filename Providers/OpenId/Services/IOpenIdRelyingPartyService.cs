using DotNetOpenAuth.OpenId.RelyingParty;
using Orchard;

namespace NGM.OpenAuthentication.Providers.OpenId.Services {
    public interface IOpenIdRelyingPartyService : IDependency {
        IAuthenticationResponse Response { get; }
        IAuthenticationRequest CreateRequest(OpenIdIdentifier openIdIdentifier);
        bool HasResponse { get; }
    }
}