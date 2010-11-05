using Orchard.ContentManagement;
using Orchard.Security;

namespace NGM.OpenAuthentication.Core.OpenId {
    public class OpenIdUser : IUser {
        private ContentItem _contentItem;
        private int _id;
        private string _userName;
        private string _email;

        public OpenIdUser(string userName) {
            _userName = userName;
        }

        public ContentItem ContentItem {
            get { return _contentItem; }
        }

        public int Id {
            get { return _id; }
        }

        public string UserName {
            get { return _userName; }
        }

        public string Email {
            get { return _email; }
        }
    }
}