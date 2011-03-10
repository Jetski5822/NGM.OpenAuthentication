using Orchard.UI.Resources;

namespace NGM.OpenAuthentication {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("Account").SetUrl("account.css");

            manifest.DefineStyle("openIdStyles").SetUrl("openid.css").SetVersion("1.3b1");
            manifest.DefineScript("openIdSelector").SetUrl("openid-jquery-1.3b1.js").SetVersion("1.3b1").SetDependencies("jQuery");
            manifest.DefineScript("openIdLocalization").SetUrl("openid-en.js").SetDependencies("openIdSelector").SetVersion("1.3b1");
        }
    }
}