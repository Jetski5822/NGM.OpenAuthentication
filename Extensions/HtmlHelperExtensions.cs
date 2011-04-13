using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Core.OAuth;

namespace NGM.OpenAuthentication.Extensions {
    public static class HtmlHelperExtensions {
        public static string TranslateProvider(this HtmlHelper htmlHelper, int hashedProvider) {
            if (hashedProvider == ProviderHelpers.GetHashedProvider(Provider.Facebook)) {
                return "Facebook";
            }
            if (hashedProvider == ProviderHelpers.GetHashedProvider(Provider.LiveId)) {
                return "Microsoft Connect";
            }
            if (hashedProvider == ProviderHelpers.GetHashedProvider(Provider.Twitter)) {
                return "Twitter";
            }
            if (hashedProvider == ProviderHelpers.GetHashedProvider(Provider.OpenId)) {
                return "Open Id";
            }
            return "Unknown Provider";
        }
    }
}