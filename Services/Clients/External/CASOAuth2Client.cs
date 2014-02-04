using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using Orchard.Validation;

namespace NGM.OpenAuthentication.Services.Clients.External {
    /// <summary>
    /// Authenticate with another CAS server using OAuth v2.0 protocol wrapper
    /// </summary>
    public class CASOAuth2Client : OAuth2Client {
        /// <summary>
        /// The _app id.
        /// </summary>
        private readonly string appId;

        /// <summary>
        /// The _app secret.
        /// </summary>
        private readonly string appSecret;

        /// <summary>
        /// the _provider Identifier
        /// </summary>
        private string providerIdentifier;

        public CASOAuth2Client(string appId, string appSecret, string providerIdentifier)
			: base("cas") {
			Argument.ThrowIfNullOrEmpty(appId, "appId");
            Argument.ThrowIfNullOrEmpty(appSecret, "appSecret");
            Argument.ThrowIfNullOrEmpty(providerIdentifier, "providerIdentifier");

			this.appId = appId;
			this.appSecret = appSecret;
            this.providerIdentifier = providerIdentifier;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl) {
            throw new NotImplementedException();
        }

        protected override IDictionary<string, string> GetUserData(string accessToken) {
            throw new NotImplementedException();
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode) {
            throw new NotImplementedException();
        }
    }
}