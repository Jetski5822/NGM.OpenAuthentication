using System.Globalization;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace NGM.OpenAuthentication.Providers.MicrosoftConnect {
    [OrchardFeature("MicrosoftConnect")]
    public class MicrosoftConnectManifest : IResourceManifestProvider {
        private readonly IWorkContextAccessor _workContextAccessor;

        public MicrosoftConnectManifest(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            RegisterLocalizedVersionOfMicrosoftConnectJavascriptFiles(manifest);
        }

        private void RegisterLocalizedVersionOfMicrosoftConnectJavascriptFiles(Orchard.UI.Resources.ResourceManifest manifest) {
            string currentCultureName = _workContextAccessor.GetContext().CurrentSite.SiteCulture;

            if (string.IsNullOrEmpty(currentCultureName)) {
                RegisterDefaultLocalizedVersionOfMicrosoftConnectJavascriptFiles(manifest);
            } else {
                try {
                    var ci = new CultureInfo(currentCultureName);
                    manifest.DefineScript("MicrosoftConnect").SetUrl(string.Format("https://js.live.net/v5.0/{0}/wl.js", ci.Name)).SetVersion("5.0");
                }
                catch (CultureNotFoundException) {
                    RegisterDefaultLocalizedVersionOfMicrosoftConnectJavascriptFiles(manifest);
                }
            }
        }

        private static void RegisterDefaultLocalizedVersionOfMicrosoftConnectJavascriptFiles(Orchard.UI.Resources.ResourceManifest manifest) {
            manifest.DefineScript("MicrosoftConnect").SetUrl("https://js.live.net/v5.0/wl.js").SetVersion("5.0");
        }
    }
}