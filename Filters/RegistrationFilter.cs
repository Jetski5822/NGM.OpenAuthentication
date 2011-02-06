﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
﻿using NGM.OpenAuthentication.Models;
﻿using Orchard.Mvc.Filters;

namespace NGM.OpenAuthentication.Filters {
    public class RegistrationFilter : FilterProvider, IResultFilter {
        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (!filterContext.RouteData.Values.ContainsValue("Register"))
                return;

            var model = filterContext.Controller.TempData["registermodel"] as RegisterModel;

            if (model == null)
                return;

            filterContext.Controller.ViewData["email"] = model.Email;
            filterContext.Controller.ViewData["username"] = model.UserName;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
            
        }
    }
}