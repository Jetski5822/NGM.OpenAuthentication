using System.Web.Routing;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Core {
    public static class RouteValuesHelper
    {
        public static RouteValueDictionary CreateRegisterRouteValueDictionary(string returnUrl, RegisterModel model) {
            object routeValues = new {
                area = "Orchard.Users",
                ReturnUrl = returnUrl,
                externalidentifier = model.Parameters.ExternalIdentifier,
                externaldisplayidentifier = model.Parameters.ExternalDisplayIdentifier,
                provider = model.Parameters.HashedProvider
            };

            var dictionary = new RouteValueDictionary(routeValues);
            if (!string.IsNullOrEmpty(model.Parameters.OAuthToken))
                dictionary.Add("oauthtoken", model.Parameters.OAuthToken);
            if (!string.IsNullOrEmpty(model.Parameters.OAuthToken))
                dictionary.Add("oauthaccesstoken", model.Parameters.OAuthAccessToken);

            return dictionary;
        }
    }
}