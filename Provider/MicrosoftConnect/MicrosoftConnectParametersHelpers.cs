using System;
using System.Web;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Provider.MicrosoftConnect {
    [OrchardFeature("MicrosoftConnect")]
    public static class MicrosoftConnectParametersHelpers {
        public static string Callback() {
            UriBuilder callbackUriBuilder = new UriBuilder(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
            callbackUriBuilder.Path = "/OAuth/LogOn/" + new MicrosoftConnectAccessControlProvider().Name;
            //callbackUriBuilder.Query = SessionIdParameter();
            return callbackUriBuilder.Uri.ToString();
        }
    }
}