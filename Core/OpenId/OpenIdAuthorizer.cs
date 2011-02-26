using System.Collections.Generic;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace NGM.OpenAuthentication.Core.OpenId {
    public class OpenIdProviderAuthorizer : IOpenIdProviderAuthorizer {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthorizer _authorizer;

        public OpenIdProviderAuthorizer(IOpenIdRelyingPartyService openIdRelyingPartyService,
            IAuthorizer authorizer) {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authorizer = authorizer;
        }

        public AuthorizeState Authorize(string returnUrl) {
            switch (_openIdRelyingPartyService.Response.Status) {
                case AuthenticationStatus.Authenticated:
                    var parameters = new OpenIdAuthenticationParameters(_openIdRelyingPartyService.Response.ClaimedIdentifier, _openIdRelyingPartyService.Response.FriendlyIdentifierForDisplay);
                    var status = _authorizer.Authorize(parameters);

                    return new AuthorizeState(returnUrl, status) {
                        Error = _authorizer.Error,
                        RegisterModel = new RegisterModelBuilder(_openIdRelyingPartyService.Response).Build()
                    };
                case AuthenticationStatus.Canceled:
                    return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                        Error = new KeyValuePair<string, string>("Provider", "Canceled at provider")
                    };
                case AuthenticationStatus.Failed:
                    return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                        Error = new KeyValuePair<string, string>("Provider", _openIdRelyingPartyService.Response.Exception.Message)
                    };
            }

            return new AuthorizeState(returnUrl, OpenAuthenticationStatus.Unknown);
        }

        public bool IsOpenIdCallback {
            get { return _openIdRelyingPartyService.HasResponse; }
        }
    }
}