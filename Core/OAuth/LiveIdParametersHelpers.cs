using System;
using System.Web;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Core.OAuth {
    [OrchardFeature("MicrosoftConnect")]
    public static class LiveIdParametersHelpers {
        

        public static string LiveIdCallback() {
            UriBuilder callbackUriBuilder = new UriBuilder(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
            callbackUriBuilder.Path = "/OAuth/LogOn/" + Provider.LiveId.ToString();
            //callbackUriBuilder.Query = SessionIdParameter();
            return callbackUriBuilder.Uri.ToString();
        }
    }
}