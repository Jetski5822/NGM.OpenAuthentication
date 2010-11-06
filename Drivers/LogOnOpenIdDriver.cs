using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace NGM.OpenAuthentication.Drivers {
    public class LogOnOpenIdDriver : ContentPartDriver<OpenAuthenticationPart> {
        private readonly IContentManager _contentManager;

        public LogOnOpenIdDriver(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        protected override DriverResult Editor(OpenAuthenticationPart part, dynamic shapeHelper) {
            return ContentShape("LogOn",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts/LogOn.OpenId.Fields", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(OpenAuthenticationPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}