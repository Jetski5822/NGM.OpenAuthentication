using DotNetOpenAuth.OpenId.RelyingParty;

namespace NGM.OpenAuthentication.Core {
    public interface IAuthenticationResolver {
        void AuthenticateResponse(IAuthenticationResponse authenticationResponse);
        bool IsAccountValidFor(IAuthenticationResponse authenticationResponse);
    }
}