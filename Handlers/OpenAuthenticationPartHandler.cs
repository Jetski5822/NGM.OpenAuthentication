using System.Linq;
using System.Web.Routing;
using JetBrains.Annotations;
using NGM.OpenAuthentication.Core.OpenId;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using Orchard;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Users.Models;

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

            OnCreated<UserPart>((context, user) => {
                var cliamedIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["cliamedidentifier"];
                var friendlyIdentifier = _orchardServices.WorkContext.HttpContext.Request.Params["friendlyidentifier"];
                _openAuthenticationService.AssociateOpenIdWithUser(user, cliamedIdentifier, friendlyIdentifier);
            });
        }
    }
}