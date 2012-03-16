using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using Orchard.Mvc.Filters;

namespace NGM.OpenAuthentication.Filters {
    public class LogOutFilter : FilterProvider, IResultFilter {
        private readonly IStateBag _stateBag;

        public LogOutFilter(IStateBag stateBag) {
            _stateBag = stateBag;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {

        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
            if (filterContext.RouteData.Values["controller"] as string != "Account")
                return;
            if (filterContext.RouteData.Values["action"] as string != "LogOff")
                return;

            _stateBag.Clear();
        }
    }
}