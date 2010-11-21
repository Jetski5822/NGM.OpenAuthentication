using Orchard.UI.Resources;

namespace NGM.OpenAuthentication {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            builder.Add().DefineStyle("Account").SetUrl("account.css");
        }
    }
}
