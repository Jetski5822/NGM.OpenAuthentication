using System.Collections.Generic;
using System.Linq;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Models;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.Services.Clients;
using Orchard.Caching;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Localization;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeTableProvider {
        private readonly IEnumerable<IExternalAuthenticationClient> _openAuthAuthenticationClients;
        private readonly IOrchardOpenAuthClientProvider _orchardOpenAuthClientProvider;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public Shapes(
            IEnumerable<IExternalAuthenticationClient> openAuthAuthenticationClients,
            IOrchardOpenAuthClientProvider orchardOpenAuthClientProvider,
            ICacheManager cacheManager,
            ISignals signals) {
            _openAuthAuthenticationClients = openAuthAuthenticationClients;
            _orchardOpenAuthClientProvider = orchardOpenAuthClientProvider;
            _cacheManager = cacheManager;
            _signals = signals;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Discover(ShapeTableBuilder builder) {

            builder.Describe("LogOn")
                   .OnDisplaying(displaying => {
                        displaying.Shape.ClientsData = GetClientData();

                        displaying.ShapeMetadata.Type = "OpenAuthLogOn";
                    });

            builder.Describe("Register")
                   .OnDisplaying(displaying => {
                       displaying.Shape.ClientsData = GetClientData();

                       displaying.ShapeMetadata.Type = "OpenAuthRegister";
                   });
        }

        private List<OrchardAuthenticationClientData> GetClientData() {
            return _cacheManager.Get(Constants.CacheKey.ProviderCacheKey, context => {
                _signals.When(Constants.CacheKey.ProviderCacheKey);

                return _openAuthAuthenticationClients
                    .Select(client => _orchardOpenAuthClientProvider.GetClientData(client.ProviderName))
                    .Where(x => x != null)
                    .ToList(); 
            });
        }
    }
}
