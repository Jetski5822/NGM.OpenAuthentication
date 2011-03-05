using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGM.OpenAuthentication.Core.OAuth {
    public class LiveIdProviderAuthorizer : IOAuthProviderAuthorizer {
        public AuthorizeState Authorize(string returnUrl) {
            throw new NotImplementedException();
        }

        public string ClientKeyIdentifier {
            get { throw new NotImplementedException(); }
        }

        public string ClientSecret {
            get { throw new NotImplementedException(); }
        }

        public bool IsConsumerConfigured {
            get { throw new NotImplementedException(); }
        }

        public OAuthProvider Provider {
            get { throw new NotImplementedException(); }
        }
    }
}