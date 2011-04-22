using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
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

            State.Clear();
        }
    }
}