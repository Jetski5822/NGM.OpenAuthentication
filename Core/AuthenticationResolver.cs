using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Services;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core {
    public class AuthenticationResolver : IAuthenticationResolver {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public AuthenticationResolver(IAuthenticationService authenticationService, IOpenAuthenticationService openAuthenticationService) {
            _authenticationService = authenticationService;
            _openAuthenticationService = openAuthenticationService;
        }

        public void AuthenticateResponse(IAuthenticationResponse authenticationResponse) {
            var identifier = new OpenIdIdentifier(authenticationResponse.ClaimedIdentifier);

            var user = _openAuthenticationService.GetUserFor(identifier);
            
            _authenticationService.SignIn(user, false);
        }

        public bool IsAccountValidFor(IAuthenticationResponse authenticationResponse) {
            var identifier = new OpenIdIdentifier(authenticationResponse.ClaimedIdentifier);

            if (_openAuthenticationService.GetUserFor(identifier) != null)
                return true;

            return false;
        }
    }
}