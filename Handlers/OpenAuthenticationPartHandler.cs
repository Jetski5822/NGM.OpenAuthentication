using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Security;

namespace NGM.OpenAuthentication.Handlers {
    [UsedImplicitly]
    public class OpenAuthenticationPartHandler : ContentHandler {
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IEnumerable<IAccessControlProvider> _accessControlProviders;

        public OpenAuthenticationPartHandler(IRepository<OpenAuthenticationPartRecord> openAuthenticationPartRepository,
            IOpenAuthenticationService openAuthenticationService,
            IEnumerable<IAccessControlProvider> accessControlProviders) {
            _openAuthenticationService = openAuthenticationService;
            _accessControlProviders = accessControlProviders;
            Filters.Add(StorageFilter.For(openAuthenticationPartRepository));

            OnRemoved<IUser>(
                (context, user) => _openAuthenticationService.GetExternalIdentifiersFor(user)
                                       .List()
                                       .ToList()
                                       .ForEach(
                                           o =>
                                           _openAuthenticationService.RemoveAssociation(
                                               new HashedOpenAuthenticationParameters(
                                                   _accessControlProviders.First(x => x.Hash == o.HashedProvider),
                                                   o.ExternalIdentifier))));
        }
    }
}