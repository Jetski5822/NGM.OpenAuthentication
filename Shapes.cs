using NGM.OpenAuthentication.Services;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        private readonly IOpenAuthenticationService _openAuthenticationService;
        
        dynamic Shape { get; set; }

        public Shapes(IOpenAuthenticationService openAuthenticationService,
            IShapeFactory shapeFactory) {
            _openAuthenticationService = openAuthenticationService;
            Shape = shapeFactory;
        }

        public void Creating(ShapeCreatingContext context) {
        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn") {
                var settings = _openAuthenticationService.GetSettings();

                if (settings.Record.OpenIdEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_LogOn");
                if (settings.Record.CardSpaceEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_CardSpace_LogOn");
                if (settings.Record.OAuthEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OAuth_LogOn");
            }
            if (context.ShapeType == "Register") {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_Register");
            }
        }
    }
}