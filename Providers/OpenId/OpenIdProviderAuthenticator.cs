using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Providers.OpenId {
    [OrchardFeature("OpenId")]
    public class OpenIdProviderAuthenticator : IOpenIdProviderAuthenticator {
        private readonly IOpenIdRelyingPartyService _openIdRelyingPartyService;
        private readonly IAuthenticator _authenticator;
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;
        private readonly IOrchardServices _orchardServices;

        public OpenIdProviderAuthenticator(IOpenIdRelyingPartyService openIdRelyingPartyService,
            IAuthenticator authenticator,
            IScopeProviderPermissionService scopeProviderPermissionService,
            IOrchardServices orchardServices)
        {
            _openIdRelyingPartyService = openIdRelyingPartyService;
            _authenticator = authenticator;
            _scopeProviderPermissionService = scopeProviderPermissionService;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public AuthenticationState Authenticate(string returnUrl) {
            if (IsOpenIdCallback)
                return TranslateResponseState(returnUrl);

            return GenerateRequestState(returnUrl);
        }

        private AuthenticationState TranslateResponseState(string returnUrl) {
            switch (_openIdRelyingPartyService.Response.Status) {
                case AuthenticationStatus.Authenticated:
                    var parameters = new OpenIdAuthenticationParameters(_openIdRelyingPartyService.Response);
                    return new AuthenticationState(returnUrl, _authenticator.Authenticate(parameters).Status);
                case AuthenticationStatus.Canceled:
                    _orchardServices.Notifier.Error(T("Canceled at provider"));
                    return new AuthenticationState(returnUrl, Statuses.ErrorAuthenticating);
                case AuthenticationStatus.Failed:
                    _orchardServices.Notifier.Error(T(_openIdRelyingPartyService.Response.Exception.Message));
                    return new AuthenticationState(returnUrl, Statuses.ErrorAuthenticating);
            }
            return new AuthenticationState(returnUrl, Statuses.Unknown);
        }

        private AuthenticationState GenerateRequestState(string returnUrl) {
            var identifier = new OpenIdIdentifier(EnternalIdentifier);
            if (!identifier.IsValid) {
                _orchardServices.Notifier.Error(T("Invalid Open ID identifier"));
                return new AuthenticationState(returnUrl, Statuses.ErrorAuthenticating);
            }

            try {
                var request = _openIdRelyingPartyService.CreateRequest(identifier);

                request.AddExtension(Claims.CreateClaimsRequest(_scopeProviderPermissionService));
                request.AddExtension(Claims.CreateFetchRequest(_scopeProviderPermissionService));

                return new AuthenticationState(returnUrl, Statuses.RequresRedirect) {
                    Result = request.RedirectingResponse.AsActionResult()
                };
            } catch (ProtocolException ex) {
                _orchardServices.Notifier.Error(T("Unable to authenticate: {0}", ex.Message));
                return new AuthenticationState(returnUrl, Statuses.ErrorAuthenticating);
            }
        }

        public string EnternalIdentifier { get; set; }

        public bool IsOpenIdCallback {
            get { return _openIdRelyingPartyService.HasResponse; }
        }
    }
}