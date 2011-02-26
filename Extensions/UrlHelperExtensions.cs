using System.Web;
using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Extensions {
    public static class UrlHelperExtensions {
        public static string LogOn(this UrlHelper urlHelper, string returnUrl) {
            return urlHelper.Action("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
        }

        public static string Register(this UrlHelper urlHelper, string returnUrl, RegisterModel model) {
            return urlHelper.Action("Register", "Account", RouteValuesHelper.CreateRegisterRouteValueDictionary(returnUrl, model));
        }

        public static string Referer(this UrlHelper urlHelper, HttpRequestBase httpRequestBase) {
            return httpRequestBase.UrlReferrer.ToString();
        }
    }
}