using System.Collections.Generic;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Services;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Core.OpenId {
    [OrchardFeature("OpenId")]
    public class OpenIdProviderAuthenticator : IOpenIdProviderAuthenticator {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthenticator _authenticator;
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;

        public OpenIdProviderAuthenticator(IOpenIdRelyingPartyService openIdRelyingPartyService,
            IAuthenticator authenticator,
            IScopeProviderPermissionService scopeProviderPermissionService)
        {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authenticator = authenticator;
            _scopeProviderPermissionService = scopeProviderPermissionService;
        }

        public AuthenticationState Authenticate(string returnUrl) {
            if (IsOpenIdCallback)
                return TranslateResponseState(returnUrl);

            return GenerateRequestState(returnUrl);
        }

        private AuthenticationState TranslateResponseState(string returnUrl) {
            switch (_openIdRelyingPartyService.Response.Status) {
                case AuthenticationStatus.Authenticated:
                    var parameters = new OpenIdAuthenticationParameters(_openIdRelyingPartyService.Response);
                    return new AuthenticationState(returnUrl, _authenticator.Authorize(parameters));
                case AuthenticationStatus.Canceled:
                    return new AuthenticationState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                        Error = new KeyValuePair<string, string>("Provider", "Canceled at provider")
                    };
                case AuthenticationStatus.Failed:
                    return new AuthenticationState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                        Error = new KeyValuePair<string, string>("Provider", _openIdRelyingPartyService.Response.Exception.Message)
                    };
            }
            return new AuthenticationState(returnUrl, OpenAuthenticationStatus.Unknown);
        }

        private AuthenticationState GenerateRequestState(string returnUrl) {
            var identifier = new OpenIdIdentifier(EnternalIdentifier);
            if (!identifier.IsValid) {
                return new AuthenticationState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                    Error = new KeyValuePair<string, string>("Error", "Invalid Open ID identifier")
                };
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);

                request.AddExtension(Claims.CreateClaimsRequest(_scopeProviderPermissionService));
                request.AddExtension(Claims.CreateFetchRequest(_scopeProviderPermissionService));

                return new AuthenticationState(returnUrl, OpenAuthenticationStatus.RequresRedirect) {
                    Result = request.RedirectingResponse.AsActionResult()
                };
            } catch (ProtocolException ex) {
                return new AuthenticationState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                    Error = new KeyValuePair<string, string>("Protocol", "Unable to authenticate: " + ex.Message)
                };
            }
        }

        public string EnternalIdentifier { get; set; }

        public bool IsOpenIdCallback {
            get { return _openIdRelyingPartyService.HasResponse; }
        }
    }
}