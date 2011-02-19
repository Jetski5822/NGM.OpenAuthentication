using JetBrains.Annotations;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Security;

namespace NGM.OpenAuthentication.Handlers {
    [UsedImplicitly]
    public class OpenAuthenticationPartHandler : ContentHandler {
        private readonly IOrchardServices _orchardServices;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public OpenAuthenticationPartHandler(IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRepository,
            IOrchardServices orchardServices,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _openAuthenticationService = openAuthenticationService;
            Filters.Add(StorageFilter.For(openAuthenticationPartRepository));

            OnCreated<IUser>((context, user) => {
                var externalIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externalidentifier"];
                var externalDisplayIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externaldisplayidentifier"];
                var oAuthToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthtoken"];
                var oAuthAccessToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthaccesstoken"];
                var provider = int.Parse(_orchardServices.WorkContext.HttpContext.Request.Params["provider"]);

                if (!string.IsNullOrEmpty(externalIdentifier)) {
                    var parameters = new HashedOpenAuthenticationParameters(provider) {
                        ExternalIdentifier = externalIdentifier,
                        ExternalDisplayIdentifier = externalDisplayIdentifier,
                        OAuthToken = oAuthToken,
                        OAuthAccessToken = oAuthAccessToken
                    };

                    if (!_openAuthenticationService.AccountExists(parameters)) {
                        _openAuthenticationService.AssociateExternalAccountWithUser(user, parameters);
                    }
                }
            });
        }
    }
}