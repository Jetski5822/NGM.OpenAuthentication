using System.Web;
using System.Web.Mvc;
using Orchard.Mvc.Filters;

namespace NGM.OpenAuthentication.Filters {
    public class LogOutFilter : FilterProvider, IResultFilter {
        public void OnResultExecuting(ResultExecutingContext filterContext) {

        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
            if (filterContext.RouteData.Values["controller"] as string != "Account")
                return;
            if (filterContext.RouteData.Values["action"] as string != "LogOff")
                return;

            HttpContext.Current.Session.Remove("parameters");

            return;
        }
    }
}