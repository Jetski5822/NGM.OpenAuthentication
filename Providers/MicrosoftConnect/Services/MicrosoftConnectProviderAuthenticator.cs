using System;
using System.Linq;
using System.Web.Mvc;
using NGM.MicrosoftConnect;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OAuth;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.MicrosoftConnect.Services {
    [OrchardFeature("MicrosoftConnect")]
    public class MicrosoftConnectProviderAuthenticator : IMicrosoftConnectProviderAuthenticator {
        private readonly IOrchardServices _orchardServices;
        private readonly IAuthenticator _authenticator;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IScopeProviderPermissionService _scopeProviderPermissionService;

        public MicrosoftConnectProviderAuthenticator(IOrchardServices orchardServices,
            IAuthenticator authenticator,
            IOpenAuthenticationService openAuthenticationService,
            IScopeProviderPermissionService scopeProviderPermissionService) {
            _orchardServices = orchardServices;
            _authenticator = authenticator;
            _openAuthenticationService = openAuthenticationService;
            _scopeProviderPermissionService = scopeProviderPermissionService;
        }

        private Uri GenerateCallbackUri() {
            UriBuilder builder = new UriBuilder(_orchardServices.WorkContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            var path = _orchardServices.WorkContext.HttpContext.Request.ApplicationPath + "/OAuth/LogOn/" + Provider.ToString();
            builder.Path = path.Replace(@"//", @"/");

            return builder.Uri;
        }

        public AuthenticationState Authenticate(string returnUrl) {
            var scopes = _scopeProviderPermissionService.Get(Provider).Where(o => o.IsEnabled).Select(o => o.Scope).Select(o => new Scope(o));

            if (MicrosoftConnectOAuthRequest.HasRequestCode()) {
                MicrosoftConnectOAuthRequestBuilder builder = new MicrosoftConnectOAuthRequestBuilder(ClientKeyIdentifier, ClientSecret, GenerateCallbackUri(), ResponseType.Code, scopes);
                var response = builder.Build().GetResponse();

                var parameters = new OAuthAuthenticationParameters(Provider) {
                    ExternalIdentifier = response.AccessToken,
                    OAuthToken = response.RefreshToken,
                    OAuthAccessToken = response.AccessToken,
                };

                var result = _authenticator.Authenticate(parameters);

                var tempReturnUrl = _orchardServices.WorkContext.HttpContext.Request.QueryString["?ReturnUrl"];
                if (!string.IsNullOrEmpty(tempReturnUrl) && string.IsNullOrEmpty(returnUrl)) {
                    returnUrl = tempReturnUrl;
                }
                MicrosoftConnectClient client = new MicrosoftConnectClient(response.AccessToken);
                var obj = client.Get("me");
                var x = obj.id;

                return new AuthenticationState(returnUrl, result.Status);
            }
            
            var authorizationRequestBuilder = 
                new AuthorizationRequestBuilder(ClientKeyIdentifier, GenerateCallbackUri(), ResponseType.Code, scopes);

            return new AuthenticationState(returnUrl, Statuses.RequresRedirect) {
                Result = new RedirectResult(authorizationRequestBuilder.Build().GenerateRequestUri().ToString())
            };
        }

        public string ClientKeyIdentifier {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientIdentifier; }
        }

        public string ClientSecret {
            get { return _openAuthenticationService.GetSettings().Record.LiveIdClientSecret; }
        }

        public bool IsConsumerConfigured {
            get { return !string.IsNullOrEmpty(ClientKeyIdentifier) && !string.IsNullOrEmpty(ClientSecret); }
        }

        public AccessControlProvider Provider {
            get { return new MicrosoftConnectAccessControlProvider(); }
        }
    }
}