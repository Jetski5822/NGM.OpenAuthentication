using System.Collections.Generic;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Core.OAuth.Twitter.Services {
    [OrchardFeature("Twitter")]
    public class TwitterServiceNotificationProvider: INotificationProvider {
        private readonly IOAuthProviderTwitterAuthenticator _oAuthProviderTwitterAuthenticator;

        public TwitterServiceNotificationProvider(IOAuthProviderTwitterAuthenticator oAuthProviderTwitterAuthenticator) {
            _oAuthProviderTwitterAuthenticator = oAuthProviderTwitterAuthenticator;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IEnumerable<NotifyEntry> GetNotifications() {
            if (!_oAuthProviderTwitterAuthenticator.IsConsumerConfigured) {
                yield return new NotifyEntry { Message = T("You need to add ClientKeyIdentifier and ClientSecret to enable Twitter feature."), Type = NotifyType.Warning };
            }
        }
    }
}
