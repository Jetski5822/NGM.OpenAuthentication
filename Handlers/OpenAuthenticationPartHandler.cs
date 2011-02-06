using JetBrains.Annotations;
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
                var cliamedIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["claimedidentifier"];
                var friendlyIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["friendlyidentifier"];

                if (!string.IsNullOrEmpty(cliamedIdentifier) || !string.IsNullOrEmpty(friendlyIdentifier)) {
                    _openAuthenticationService.AssociateOpenIdWithUser(user, cliamedIdentifier, friendlyIdentifier);
                }
            });
        }
    }
}