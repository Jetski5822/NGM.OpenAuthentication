using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace NGM.OpenAuth {
    public class Routes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "OpenIdLogOn",
                                                         new RouteValueDictionary {
                                                                                      {"area", "NGM.OpenAuthentication"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "LogOn"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "NGM.OpenAuthentication"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "OpenIdRegister",
                                                         new RouteValueDictionary {
                                                                                      {"area", "NGM.OpenAuthentication"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "Register"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "NGM.OpenAuthentication"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                         };
        }
    }
}