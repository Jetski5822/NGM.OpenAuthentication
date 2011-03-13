using System.Collections.Generic;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Services;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Core.OpenId {
    [OrchardFeature("OpenId")]
    public class OpenIdProviderAuthorizer : IOpenIdProviderAuthorizer {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IAuthorizer _authorizer;

        public OpenIdProviderAuthorizer(IOpenIdRelyingPartyService openIdRelyingPartyService,
            IOpenAuthenticationService openAuthenticationService,
            IAuthorizer authorizer) {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _openAuthenticationService = openAuthenticationService;
            _authorizer = authorizer;
        }

        public AuthorizeState Authorize(string returnUrl) {
            if (IsOpenIdCallback)
                return TranslateResponseState(returnUrl);

            return GenerateRequestState(returnUrl);
        }

        private AuthorizeState TranslateResponseState(string returnUrl) {
            switch (_openIdRelyingPartyService.Response.Status) {
                case AuthenticationStatus.Authenticated:
                    var parameters = new OpenIdAuthenticationParameters(_openIdRelyingPartyService.Response);
                    return new AuthorizeState(returnUrl, _authorizer.Authorize(parameters));
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

        private AuthorizeState GenerateRequestState(string returnUrl) {
            var identifier = new OpenIdIdentifier(this.EnternalIdentifier);
            if (!identifier.IsValid) {
                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
                    Error = new KeyValuePair<string, string>("Error", "Invalid Open ID identifier")
                };
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);

                request.AddExtension(Claims.CreateClaimsRequest(_openAuthenticationService.GetSettings()));
                request.AddExtension(Claims.CreateFetchRequest(_openAuthenticationService.GetSettings()));

                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.RequresRedirect) {
                    Result = request.RedirectingResponse.AsActionResult()
                };
            } catch (ProtocolException ex) {
                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.ErrorAuthenticating) {
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