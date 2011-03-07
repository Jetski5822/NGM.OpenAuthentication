using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NGM.OpenAuthentication.Core.OAuth;
using Orchard.Mvc.Filters;

namespace NGM.OpenAuthentication.Filters {
    public class LogOutFilter : FilterProvider, IResultFilter {
        private readonly IOAuthProviderLiveIdAuthorizer _authorizer;

        public LogOutFilter(IOAuthProviderLiveIdAuthorizer authorizer) {
            _authorizer = authorizer;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
            if (filterContext.RouteData.Values["controller"] as string != "Account")
                return;
            if (filterContext.RouteData.Values["action"] as string != "LogOff")
                return;

            //var cookie = filterContext.HttpContext.Request.Cookies[LiveIdProviderAuthorizer.LoginCookie];
            //if (cookie != null) {
            //    cookie.Expires = DateTime.Now.AddYears(-10);
            //    filterContext.HttpContext.Request.Cookies.Add(cookie);
            //}
            _authorizer.LogOut(string.Empty);

            return;
        }
    }
}