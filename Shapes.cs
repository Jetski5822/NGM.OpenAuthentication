using System.Web;
using NGM.OpenAuthentication.Services;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        private readonly IOpenAuthenticationService _openAuthenticationService;

        public Shapes(IOpenAuthenticationService openAuthenticationService) {
            _openAuthenticationService = openAuthenticationService;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn") {
                if ((HttpContext.Current.Session["parameters"] as Core.OpenAuthenticationParameters) != null)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_AssociateMessage");

                var settings = _openAuthenticationService.GetSettings();

                if (settings.Record.OpenIdEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_LogOn");
                if (settings.Record.CardSpaceEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_CardSpace_LogOn");
                if (settings.Record.OAuthEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OAuth_LogOn");
            }
            if (context.ShapeType == "Register") {
                if ((HttpContext.Current.Session["parameters"] as Core.OpenAuthenticationParameters) != null)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_AssociateMessage");
            }
        }
    }
}