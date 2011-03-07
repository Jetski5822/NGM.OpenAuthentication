using System.Linq;
using JetBrains.Annotations;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OAuth;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Security;
using Orchard.UI.Notify;

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
                                 var parameters = _orchardServices.WorkContext.HttpContext.Session["parameters"] as OpenAuthenticationParameters;
                                 if (parameters == null) {
                                     var externalIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externalidentifier"];
                                     var externalDisplayIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externaldisplayidentifier"];
                                     var oAuthToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthtoken"];
                                     var oAuthAccessToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthaccesstoken"];
                                     var provider = int.Parse(_orchardServices.WorkContext.HttpContext.Request.Params["provider"]);

                                     if (!string.IsNullOrEmpty(externalIdentifier)) {
                                         parameters = new HashedOpenAuthenticationParameters(provider) {
                                             ExternalIdentifier = externalIdentifier,
                                             ExternalDisplayIdentifier = externalDisplayIdentifier,
                                             OAuthToken = oAuthToken,
                                             OAuthAccessToken = oAuthAccessToken
                                         };
                                     }
                                 }
                                 else {
                                     _orchardServices.WorkContext.HttpContext.Session.Remove("parameters");
                                 }

                                 if (parameters != null && !_openAuthenticationService.AccountExists(parameters)) {
                                     _openAuthenticationService.AssociateExternalAccountWithUser(user, parameters);
                                 }

                                 TryAssociateLiveIdParameters(user);
                             });
            
            OnLoaded<IUser>((context, user) => {
                                if (_orchardServices.WorkContext.CurrentUser != null) return;
                                
                                TryAssociateLiveIdParameters(user);
                            });

            OnRemoved<IUser>((context, user) => _openAuthenticationService.GetExternalIdentifiersFor(user)
                                                    .List()
                                                    .ToList()
                                                    .ForEach(o => _openAuthenticationService.RemoveAssociation(new HashedOpenAuthenticationParameters(o.Record.HashedProvider, o.Record.ExternalIdentifier))));
        }

        private void TryAssociateLiveIdParameters(IUser user) {
            var cookie = _orchardServices.WorkContext.HttpContext.Request.Cookies[LiveIdProviderAuthorizer.LoginCookie];
            if (cookie == null) return;

            var parameters = new HashedOpenAuthenticationParameters(OAuthProvider.LiveId.GetHashCode()) {
                ExternalIdentifier = cookie.Value,
                ExternalDisplayIdentifier = cookie.Values["UserId"],
                OAuthToken = cookie.Value
            };

            if (!_openAuthenticationService.AccountExists(parameters)) {
                _openAuthenticationService.AssociateExternalAccountWithUser(user, parameters);
            }
        }
    }
}