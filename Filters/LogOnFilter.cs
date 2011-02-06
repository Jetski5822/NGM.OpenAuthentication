//﻿using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//﻿using NGM.OpenAuthentication.Models;
//﻿using Orchard.Mvc.Filters;

//namespace NGM.OpenAuthentication.Filters {
//    public class LogOnFilter : FilterProvider, IResultFilter {
//        public void OnResultExecuting(ResultExecutingContext filterContext) {
//            if (!filterContext.RouteData.Values.ContainsValue("LogOn"))
//                return;

//            var errors = filterContext.Controller.TempData.Where(o => o.Key.StartsWith("error-")).ToArray();

//            foreach (var error in errors.Where(error => filterContext.Controller.ViewData.ModelState.ContainsKey(error.Key))) {
//                filterContext.Controller.ViewData.ModelState.AddModelError(error.Key, error.Value.ToString());
//            }
//        }

//        public void OnResultExecuted(ResultExecutedContext filterContext) {
            
//        }
//    }
//}