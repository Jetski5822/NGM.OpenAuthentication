using System.Web;
using System.Web.Mvc;

namespace NGM.OpenAuthentication.Extensions {
    public static class UrlHelperExtensions {
        public static string LogOn(this UrlHelper urlHelper, string returnUrl) {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.Action("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
            return urlHelper.Action("LogOn", "Account", new { area = "Orchard.Users" });
        }

        public static string LogOff(this UrlHelper urlHelper, string returnUrl) {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.Action("LogOff", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
            return urlHelper.Action("LogOff", "Account", new { area = "Orchard.Users" });
        }

        public static string Referer(this UrlHelper urlHelper, HttpRequestBase httpRequestBase) {
            if (httpRequestBase.UrlReferrer != null) {
                return httpRequestBase.UrlReferrer.ToString();
            }
            return "~/";
        }
    }
}