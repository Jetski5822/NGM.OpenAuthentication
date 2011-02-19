using System.Web.Mvc;
﻿using NGM.OpenAuthentication.Core;
﻿using NGM.OpenAuthentication.Models;
﻿using Orchard;
﻿using Orchard.DisplayManagement;
﻿using Orchard.Mvc.Filters;

namespace NGM.OpenAuthentication.Filters {
    public class RegistrationFilter : FilterProvider, IResultFilter {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly dynamic _shapeFactory;

        public RegistrationFilter(IWorkContextAccessor workContextAccessor,
            IShapeFactory shapeFactory) {
            _workContextAccessor = workContextAccessor;
            _shapeFactory = shapeFactory;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (!filterContext.RouteData.Values.ContainsValue("Register"))
                return;

            if (!OrchardShapeChecker.HasRegisterAsShape()) {
                var zone = _workContextAccessor.GetContext(filterContext).Layout.Zones["BeforeContent"]; ;

                zone.Add(_shapeFactory.Wrappers_Account_Register());
            }

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