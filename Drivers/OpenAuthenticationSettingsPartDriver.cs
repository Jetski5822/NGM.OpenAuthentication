using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;

namespace NGM.OpenAuthentication.Drivers {
    public class OpenAuthenticationSettingsPartDriver : ContentPartDriver<OpenAuthenticationSettingsPart>
    {
        public OpenAuthenticationSettingsPartDriver()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "OpenAuthenticationSettings"; } }

        protected override DriverResult Editor(OpenAuthenticationSettingsPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(OpenAuthenticationSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            return ContentShape("Parts_OpenAuthentication_SiteSettings", () =>
            {
                    if (updater != null) {
                        updater.TryUpdateModel(part.Record, Prefix, null, null);
                    }
                    return shapeHelper.EditorTemplate(TemplateName: "Parts.OpenAuthentication.SiteSettings", Model: part.Record, Prefix: Prefix); 
                })
                .OnGroup("openauthentication");
        }
    }
}