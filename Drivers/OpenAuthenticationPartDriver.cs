using NGM.OpenAuthentication.Models;
using Orchard.ContentManagement.Drivers;

namespace NGM.OpenAuthentication.Drivers {
    public class OpenAuthenticationPartDriver : ContentPartDriver<OpenAuthenticationPart> {
        protected override string Prefix {
            get {
                return "OpenAuthentication";
            }
        }
    }
}