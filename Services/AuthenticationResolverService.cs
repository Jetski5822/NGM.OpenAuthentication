using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OpenId.RelyingParty;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public class AuthenticationResolverService : IAuthenticationResolverService {
        private readonly IMembershipService _membershipService;

        public AuthenticationResolverService(IMembershipService membershipService) {
            _membershipService = membershipService;
        }

        public void AuthenticateResponse(IAuthenticationResponse authenticationResponse) {
            
        }

        public bool IsAccountValidFor(IAuthenticationResponse authenticationResponse) {
            return false;
        }
    }
}