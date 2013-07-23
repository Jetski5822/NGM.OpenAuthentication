using DotNetOpenAuth.AspNet;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;

namespace NGM.OpenAuthentication.Services {
    public interface IOrchardOpenAuthDataProvider : IOpenAuthDataProvider, IDependency {
    }

    public class OrchardOpenAuthDataProvider : IOrchardOpenAuthDataProvider {
        private readonly IContentManager _contentManager;
        private readonly IUserProviderServices _userProviderServices;

        public OrchardOpenAuthDataProvider(IContentManager contentManager, IUserProviderServices userProviderServices) {
            _contentManager = contentManager;
            _userProviderServices = userProviderServices;
        }

        public string GetUserNameFromOpenAuth(string provider, string providerUserId) {
            var record = _userProviderServices.Get(provider, providerUserId);

            if (record == null)
                return null;

            var contentItem = _contentManager.Get(record.UserId);

            if (contentItem == null || !contentItem.Is<IUser>())
                return null; // May want to throw an exception here

            return contentItem.As<IUser>().UserName;
        }
    }
}