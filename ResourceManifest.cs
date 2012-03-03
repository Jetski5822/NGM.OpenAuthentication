using Orchard.UI.Resources;

namespace NGM.OpenAuthentication {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("Account").SetUrl("account.css");
        }
    }
}