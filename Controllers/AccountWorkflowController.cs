using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NGM.OpenAuthentication.Activities;
using NGM.OpenAuthentication.Extensions;
using NGM.OpenAuthentication.Mvc;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Workflows.Services;

namespace NGM.OpenAuthentication.Controllers {
    [Themed]
    [OrchardFeature("NGM.OpenAuthentication.Workflows")]
    public class AccountWorkflowController : Controller {
        private readonly IWorkflowManager _workflowManager;

        public AccountWorkflowController(IWorkflowManager workflowManager) {
            _workflowManager = workflowManager;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        [HttpPost]
        [AlwaysAccessible]
        public ActionResult ExternalLogOn(string providerName, string returnUrl) {
            return new OpenAuthLoginResult(providerName, Url.OpenAuthLogOn(returnUrl));
        }

        [AlwaysAccessible]
        public ActionResult ExternalLogOn(string returnUrl) {
            _workflowManager.TriggerEvent(OAuthVerify.ActivityName, null, () => new Dictionary<string, object>());
            return RedirectOrEmpty(() => Redirect(Url.LogOn(returnUrl)));
        }

        private ActionResult RedirectOrEmpty(Func<ActionResult> redirect) {
            return Response.IsRequestBeingRedirected ? new EmptyResult() : redirect();
        }
    }
}