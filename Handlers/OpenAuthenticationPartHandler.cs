using System;
using System.Collections.Generic;
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
        private readonly IEnumerable<IAccessControlProvider> _accessControlProviders;

        private static readonly Object SyncLock = new Object();

        public OpenAuthenticationPartHandler(IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRepository,
            IOrchardServices orchardServices,
            IOpenAuthenticationService openAuthenticationService,
            IEnumerable<IAccessControlProvider> accessControlProviders) {
            _orchardServices = orchardServices;
            _openAuthenticationService = openAuthenticationService;
            _accessControlProviders = accessControlProviders;
            Filters.Add(StorageFilter.For(openAuthenticationPartRepository));

            OnLoaded<IUser>((context, user) => {
                                if (!_orchardServices.WorkContext.HttpContext.Request.IsAuthenticated)
                                    return;
                             
                                lock (SyncLock) {
                                    if (!_orchardServices.WorkContext.HttpContext.Request.IsAuthenticated)
                                        return;

                                    if (HasQueryParamsLocator()) {
                                        TryAssociateAccount(user, GetQueryStringParameters());
                                    }
                                    else {
                                        var parameters = State.Parameters;
                                        if (parameters != null) {
                                            State.Clear();
                                            TryAssociateAccount(user, parameters);
                                        }
                                    }
                                }
                            });

            OnRemoved<IUser>((context, user) => _openAuthenticationService.GetExternalIdentifiersFor(user)
                                                    .List()
                                                    .ToList()
                                                    .ForEach(o => 
                                                        _openAuthenticationService.RemoveAssociation(
                                                            new HashedOpenAuthenticationParameters(_accessControlProviders.First(x => x.Hash == o.HashedProvider), o.ExternalIdentifier))));
        }

        // TODO Move to more appropriate location
        private OpenAuthenticationParameters GetQueryStringParameters() {
            var externalIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externalidentifier"];
            var externalDisplayIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["externaldisplayidentifier"];
            var oAuthToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthtoken"];
            var oAuthAccessToken = _orchardServices.WorkContext.HttpContext.Request.Params["oauthaccesstoken"];
            var provider = _orchardServices.WorkContext.HttpContext.Request.Params["provider"];

            return new HashedOpenAuthenticationParameters(_accessControlProviders.First(x => x.Hash == provider)) {
                ExternalIdentifier = externalIdentifier,
                ExternalDisplayIdentifier = externalDisplayIdentifier,
                OAuthToken = oAuthToken,
                OAuthAccessToken = oAuthAccessToken
            };
        }

        private bool HasQueryParamsLocator() {
            return !string.IsNullOrEmpty(_orchardServices.WorkContext.HttpContext.Request.Params["provider"]);
        }

        private void TryAssociateAccount(IUser user, OpenAuthenticationParameters parameters) {
            if (parameters != null && !_openAuthenticationService.AccountExists(parameters)) {
                _openAuthenticationService.AssociateExternalAccountWithUser(user, parameters);
            }
        }
    }
}