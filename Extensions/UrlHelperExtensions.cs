using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace NGM.OpenAuthentication.Extensions {
    public static class UrlHelperExtensions {
        public static string LogOn(this UrlHelper urlHelper, string returnUrl) {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.Action("LogOn", "Account", new { area = Constants.OrchardUsersArea, ReturnUrl = returnUrl });
            return urlHelper.Action("LogOn", "Account", new { area = Constants.OrchardUsersArea });
        }

        public static string LogOn(this UrlHelper urlHelper, string returnUrl, string userName, string loginData) {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.Action("LogOn", "Account", new { area = Constants.OrchardUsersArea, ReturnUrl = returnUrl, UserName = userName, ExternalLoginData = loginData });
            return urlHelper.Action("LogOn", "Account", new { area = Constants.OrchardUsersArea, UserName = userName, ExternalLoginData = loginData });
        }

        public static string LogOff(this UrlHelper urlHelper, string returnUrl) {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.Action("LogOff", "Account", new { area = Constants.OrchardUsersArea, ReturnUrl = returnUrl });
            return urlHelper.Action("LogOff", "Account", new { area = Constants.OrchardUsersArea });
        }

        public static string OpenAuthLogOn(this UrlHelper urlHelper, string returnUrl) {
            return urlHelper.Action("ExternalLogOn", "Account", new { area = Constants.LocalArea, ReturnUrl = returnUrl });
        }

        public static string Register(this UrlHelper urlHelper, string userName, string loginData) {
            return urlHelper.Action("Register", "Account", new { area = Constants.OrchardUsersArea, UserName = userName, ExternalLoginData = loginData });
        }

        public static string Referer(this UrlHelper urlHelper, HttpRequestBase httpRequestBase) {
            if (httpRequestBase.UrlReferrer != null) {
                return httpRequestBase.UrlReferrer.ToString();
            }
            return "~/";
        }

        public static string RemoveProviderConfiguration(this UrlHelper urlHelper, int id) {
            return urlHelper.Action("Remove", "Admin", new { area = Constants.LocalArea, Id = id });
        }

        public static string ProviderCreate(this UrlHelper urlHelper) {
            return urlHelper.Action("CreateProvider", "Admin", new { area = Constants.LocalArea });
        }
    }
}