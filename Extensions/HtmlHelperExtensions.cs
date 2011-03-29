using System.Web.Mvc;
using NGM.OpenAuthentication.Core.OAuth;

namespace NGM.OpenAuthentication.Extensions {
    public static class HtmlHelperExtensions {
        public static string TranslateProvider(this HtmlHelper htmlHelper, int hashedProvider) {
            if (hashedProvider == OAuthProvider.Facebook.ToString().GetHashCode()) {
                return "Facebook";
            }
            if (hashedProvider == OAuthProvider.LiveId.ToString().GetHashCode()) {
                return "Microsoft Connect";
            }
            if (hashedProvider == OAuthProvider.Twitter.ToString().GetHashCode()) {
                return "Twitter";
            }
            if (hashedProvider == "OpenId".ToString().GetHashCode()) {
                return "Open Id";
            }
            return "Unknown Provider";
        }
    }
}