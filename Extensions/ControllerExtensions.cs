using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NGM.OpenAuthentication.Models;
using Orchard.Localization;

namespace NGM.OpenAuthentication.Extensions {
    public static class ControllerExtensions {
        public static void AddError(this ControllerBase controllerBase, string key, LocalizedString value) {
            var errorKey = string.Format("error-{0}", key);

            if (!controllerBase.TempData.ContainsKey(errorKey)) {
                controllerBase.TempData.Add(errorKey, value);
                controllerBase.ViewData.ModelState.AddModelError(errorKey, value.Text);
            } else {
                controllerBase.TempData[errorKey] = value;
            }
        }
    }
}