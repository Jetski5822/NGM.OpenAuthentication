using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace NGM.OpenAuthentication {
    public class Routes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "OpenId/LogOn",
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
                                                         "OpenId/Register",
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
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "OpenId/VerifiedAccounts",
                                                         new RouteValueDictionary {
                                                                                      {"area", "NGM.OpenAuthentication"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "VerifiedAccounts"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "NGM.OpenAuthentication"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "OpenId/RemoveVerifiedAccount",
                                                         new RouteValueDictionary {
                                                                                      {"area", "NGM.OpenAuthentication"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "RemoveVerifiedAccount"}
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