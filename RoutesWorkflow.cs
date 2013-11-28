using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using NGM.OpenAuthentication.Extensions;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace NGM.OpenAuthentication {
    [OrchardFeature("NGM.OpenAuthentication.Workflows")]
    public class RoutesWorkflow : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            yield return new RouteDescriptor {
                Priority = 50,
                Route = new Route(
                    "External/LogOn",
                    new RouteValueDictionary {
                        {"area", Constants.LocalArea},
                        {"controller", "AccountWorkflow"},
                        {"action", "ExternalLogOn"}
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary {
                        {"area", Constants.LocalArea}
                    },
                    new MvcRouteHandler())
            };
        }
    }
}