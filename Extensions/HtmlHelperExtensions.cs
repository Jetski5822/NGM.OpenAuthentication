using System.Web.Mvc;
using NGM.OpenAuthentication.Core;

namespace NGM.OpenAuthentication.Extensions {
    public static class HtmlHelperExtensions {
        public static string TranslateProvider(this HtmlHelper htmlHelper, int hashedProvider) {
            return ProviderHelpers.GetUserFriendlyStringForHashedProvider(hashedProvider);
        }
    }
}