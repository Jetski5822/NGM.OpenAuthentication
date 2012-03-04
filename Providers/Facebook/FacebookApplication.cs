using Facebook;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Facebook {
    [OrchardFeature("Facebook")]
    public class FacebookApplication : IFacebookApplication {
        public FacebookApplication(string clientKeyIdentifier, string clientSecret) {
            AppId = clientKeyIdentifier;
            AppSecret = clientSecret;
        }

        public string AppId { get; private set; }
        public string AppSecret { get; private set; }
        public string SiteUrl { get { return null; } }
        public string CanvasPage { get { return null; } }
        public string CanvasUrl { get { return null; } }
        public string SecureCanvasUrl { get { return null; } }
        public string CancelUrlPath { get { return null; } }
        public bool UseFacebookBeta { get { return false; } }

        public bool IsSecureConnection { get; set; }
    }
}