using System.Collections.Generic;
using System.Text.RegularExpressions;
using Orchard.Events;
using Orchard.Security;
using Orchard.Users.Models;

namespace NGM.OpenAuthentication.Events {
    public interface IOpenAuthUserEventHandler : IEventHandler {
        /// <summary>
        /// Called before a User is created
        /// </summary>
        void Creating(CreatingOpenAuthUserContext context);

        /// <summary>
        /// Called after a user has been created
        /// </summary>
        void Created(CreatedOpenAuthUserContext context);
    }

    public class CreatingOpenAuthUserContext {
        public CreatingOpenAuthUserContext(string userName, string emailAddress, string providerName, string providerUserId, IDictionary<string, string> extraData) {
            UserName = userName;
            EmailAddress = emailAddress;
            ProviderName = providerName;
            ProviderUserId = providerUserId;
            ExtraData = extraData;
        }

        public string UserName { get; private set; }
        public string EmailAddress { get; set; }
        public string ProviderName { get; private set; }
        public string ProviderUserId { get; private set; }
        public IDictionary<string, string> ExtraData { get; private set; }
    }

    public class CreatedOpenAuthUserContext {
        public CreatedOpenAuthUserContext(IUser user, string providerName, string providerUserId, IDictionary<string, string> extraData) {
            User = user;
            ProviderName = providerName;
            ProviderUserId = providerUserId;
            ExtraData = extraData;
        }

        public IUser User { get; private set; }
        public string ProviderName { get; private set; }
        public string ProviderUserId { get; private set; }
        public IDictionary<string, string> ExtraData { get; private set; }
    }
}