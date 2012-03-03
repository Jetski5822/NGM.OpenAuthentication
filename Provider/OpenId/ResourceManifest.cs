using System.Globalization;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace NGM.OpenAuthentication.Provider.OpenId {
    [OrchardFeature("OpenId")]
    public class ResourceManifest : IResourceManifestProvider
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public ResourceManifest(IWorkContextAccessor workContextAccessor)
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
}