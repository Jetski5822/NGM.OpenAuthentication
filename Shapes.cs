using NGM.OpenAuthentication.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Implementation;

namespace NGM.OpenAuthentication {
    public class Shapes : IShapeFactoryEvents {
        private readonly IOrchardServices _orchardServices;

        public Shapes(IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
        }

        public void Creating(ShapeCreatingContext context) {

        }

        public void Created(ShapeCreatedContext context) {
            if (context.ShapeType == "LogOn") {
                var settings = _orchardServices.WorkContext.CurrentSite.As<OpenAuthenticationSettingsPart>();
                if (settings.Record.OpenIdEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_LogOn");
                if (settings.Record.CardSpaceEnabled)
                    context.Shape.Metadata.Wrappers.Add("Wrappers_Account_CardSpace_LogOn");
            }
            if (context.ShapeType == "Register") {
                context.Shape.Metadata.Wrappers.Add("Wrappers_Account_OpenID_Register");
            }
        }
    }
}