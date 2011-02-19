using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Extensions {
    public static class ControllerExtensions {
        public static void AddError(this ControllerBase controllerBase, string key, string value) {
            var errorKey = string.Format("error-{0}", key);

            if (!controllerBase.TempData.ContainsKey(errorKey)) {
                controllerBase.TempData.Add(errorKey, value);
                controllerBase.ViewData.ModelState.AddModelError(errorKey, value);
            } else {
                controllerBase.TempData[errorKey] = value;
            }
        }

        private static ActionResult DefaultLogOnResult(this Controller controller, string returnUrl) {
            return controller.RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = returnUrl });
        }

        private static ActionResult DefaultRegisterResult(this ControllerBase controllerBase, string returnUrl, RegisterModel model) {
            return RedirectToAction("Register", "Account", new {
                area = "Orchard.Users",
                ReturnUrl = returnUrl,
                externalidentifier = model.ExternalIdentifier,
                externaldisplayidentifier = model.ExternalDisplayIdentifier
            });
        }
    }
}