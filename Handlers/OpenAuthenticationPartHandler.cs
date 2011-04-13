using System;
using System.Linq;
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

        private static readonly Object _syncLock = new Object();

        public OpenAuthenticationPartHandler(IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRepository,
            IOrchardServices orchardServices,
            IOpenAuthenticationService openAuthenticationService) {
            _orchardServices = orchardServices;
            _openAuthenticationService = openAuthenticationService;
            Filters.Add(StorageFilter.For(openAuthenticationPartRepository));
            
            OnLoaded<IUser>((context, user) => {
                                lock (_syncLock) {
                                    if (!_orchardServices.WorkContext.HttpContext.Request.IsAuthenticated)
                                        return;

                                    if (HasQueryParamsLocator()) {
                                        TryAssociateAccount(user, GetQueryStringParameters());
                                    }

                                    var parameters = NGM.OpenAuthentication.Core.Authorizer.RetrieveParametersFromRoundTrip(true);
                                    if (parameters != null) {
                                        TryAssociateAccount(user, parameters);
                                    }
                                }
                            });

            OnRemoved<IUser>((context, user) => _openAuthenticationService.GetExternalIdentifiersFor(user)
                                                    .List()
                                                    .ToList()
                                                    .ForEach(o => _openAuthenticationService.RemoveAssociation(new HashedOpenAuthenticationParameters(o.Record.HashedProvider, o.Record.ExternalIdentifier))));
        }

        // TODO Move to more appropriate location
        private OpenAuthenticationParameters GetQueryStringParameters() {
            var externalIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externalidentifier"];
            var externalDisplayIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externaldisplayidentifier"];
            var oAuthToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthtoken"];
            var oAuthAccessToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthaccesstoken"];
            var provider = int.Parse(_orchardServices.WorkContext.HttpContext.Request.Params["provider"]);

            return new HashedOpenAuthenticationParameters(provider) {
                ExternalIdentifier = externalIdentifier,
                ExternalDisplayIdentifier = externalDisplayIdentifier,
                OAuthToken = oAuthToken,
                OAuthAccessToken = oAuthAccessToken
            };
        }

        private bool HasQueryParamsLocator() {
            return !string.IsNullOrEmpty(_orchardServices.WorkContext.HttpContext.Request.Params["externalidentifier"] as string);
        }

        private void TryAssociateAccount(IUser user, OpenAuthenticationParameters parameters) {
            if (parameters != null && !_openAuthenticationService.AccountExists(parameters)) {
                _openAuthenticationService.AssociateExternalAccountWithUser(user, parameters);
            }
        }
    }
}