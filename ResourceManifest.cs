using System;
using System.Globalization;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace NGM.OpenAuthentication {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("Account").SetUrl("account.css");
        }
    }

    [OrchardFeature("OpenId")]
    public class OpenIdResourceManifest : IResourceManifestProvider
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public OpenIdResourceManifest(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        private enum AvailableOpenIdTranslations
        {
            en,
            de,
            jp,
            ru,
            uk
        }

        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("openIdStyles").SetUrl("openid.css").SetVersion("1.3b1");
            manifest.DefineScript("openIdSelector").SetUrl("openid-jquery-1.3b1.js").SetVersion("1.3b1").SetDependencies("jQuery");
            RegisterLocalizedVersionOfOpenIdJavascriptFiles(manifest);
        }

        private void RegisterLocalizedVersionOfOpenIdJavascriptFiles(Orchard.UI.Resources.ResourceManifest manifest)
        {
            string currentCultureName = _workContextAccessor.GetContext().CurrentSite.SiteCulture;

            if (string.IsNullOrEmpty(currentCultureName)) {
                RegisterDefaultLocalizedVersionOfOpenIdJavascriptFiles(manifest);
            }
            else {
                try
                {
                    var ci = new CultureInfo(currentCultureName);

                    AvailableOpenIdTranslations valueOut;
                    if (AvailableOpenIdTranslations.TryParse(ci.TwoLetterISOLanguageName, false, out valueOut)) {
                        manifest.DefineScript("openIdLocalization").SetUrl(string.Format("openid-{0}.js", valueOut)).SetDependencies("openIdSelector").SetVersion("1.3b1");
                    }
                    else {
                        RegisterDefaultLocalizedVersionOfOpenIdJavascriptFiles(manifest);
                    }
                }
                catch (CultureNotFoundException)
                {
                    RegisterDefaultLocalizedVersionOfOpenIdJavascriptFiles(manifest);
                }
            }
        }

        private static void RegisterDefaultLocalizedVersionOfOpenIdJavascriptFiles(Orchard.UI.Resources.ResourceManifest manifest) {
            manifest.DefineScript("openIdLocalization").SetUrl("openid-en.js").SetDependencies("openIdSelector").SetVersion("1.3b1");
        }
    }

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
                    manifest.DefineScript("MicrosoftLiveConnect").SetUrl(string.Format("https://js.live.net/v5.0/{0}/wl.js", ci.Name)).SetVersion("5.0");
                }
                catch (CultureNotFoundException) {
                    RegisterDefaultLocalizedVersionOfMicrosoftConnectJavascriptFiles(manifest);
                }
            }
        }

        private static void RegisterDefaultLocalizedVersionOfMicrosoftConnectJavascriptFiles(Orchard.UI.Resources.ResourceManifest manifest) {
            manifest.DefineScript("MicrosoftLiveConnect").SetUrl("https://js.live.net/v5.0/wl.js").SetVersion("5.0");
        }
    }
}