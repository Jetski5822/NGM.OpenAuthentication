using DotNetOpenAuth.OpenId.RelyingParty;

namespace NGM.OpenAuthentication.Services {
    public interface IAuthenticationResolverService {
        void AuthenticateResponse(IAuthenticationResponse authenticationResponse);
        bool IsAccountValidFor(IAuthenticationResponse authenticationResponse);
    }
}