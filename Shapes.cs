using System.Collections.Generic;
using System.Linq;
using NGM.OpenAuthentication.Services;
using NGM.OpenAuthentication.Services.Clients;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Localization;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeTableProvider {
        private readonly IEnumerable<IExternalAuthenticationClient> _openAuthAuthenticationClients;
        private readonly IOrchardOpenAuthClientProvider _orchardOpenAuthClientProvider;

        public Shapes(
            IEnumerable<IExternalAuthenticationClient> openAuthAuthenticationClients,
            IOrchardOpenAuthClientProvider orchardOpenAuthClientProvider) {
            _openAuthAuthenticationClients = openAuthAuthenticationClients;
            _orchardOpenAuthClientProvider = orchardOpenAuthClientProvider;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Discover(ShapeTableBuilder builder) {


            builder.Describe("LogOn")
                   .OnDisplaying(displaying => {
                       var clientsData = _openAuthAuthenticationClients
                .Select(client => _orchardOpenAuthClientProvider.GetClientData(client.ProviderName))
                .Where(x => x != null)
                .ToList();
                        var shape = displaying.Shape;
                        var metadata = displaying.ShapeMetadata;

                        shape.ClientsData = clientsData;

                        metadata.Type = "OpenAuthLogOn";
                    });

            builder.Describe("Register")
                   .OnDisplaying(displaying => {
                       var clientsData = _openAuthAuthenticationClients
                           .Select(client => _orchardOpenAuthClientProvider.GetClientData(client.ProviderName))
                           .Where(x => x != null)
                           .ToList();

                       var shape = displaying.Shape;
                       var metadata = displaying.ShapeMetadata;

                       shape.ClientsData = clientsData;

                       metadata.Type = "OpenAuthRegister";
                   });
        }
    }
}
