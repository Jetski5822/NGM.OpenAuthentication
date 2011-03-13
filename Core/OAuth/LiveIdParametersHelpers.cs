using System;
using System.Web;
using Microsoft.Live;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Core.OAuth {
    [OrchardFeature("MicrosoftConnect")]
    public static class LiveIdParametersHelpers {
        public static string SessionIdParameter() { return string.Format("wl_session_id={0}", new SessionIdProvider().GetSessionId(HttpContext.Current)); }

        public static string LiveIdCallback() {
            UriBuilder callbackUriBuilder = new UriBuilder(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
            callbackUriBuilder.Path = "/OAuth/LogOn/LiveId";
            callbackUriBuilder.Query = SessionIdParameter();
            return callbackUriBuilder.Uri.ToString();
        }
    }
}