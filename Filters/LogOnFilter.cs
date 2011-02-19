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

//            filterContext.HttpContext.Request.Params.Remove("externalidentifier");
//            filterContext.HttpContext.Request.Params.Remove("externaldisplayidentifier");
//        }

//        public void OnResultExecuted(ResultExecutedContext filterContext) {

//        }
//    }
//}