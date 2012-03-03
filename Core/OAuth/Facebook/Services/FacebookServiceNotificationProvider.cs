using System.Collections.Generic;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;

namespace NGM.OpenAuthentication.Core.OAuth.Facebook.Services {
    [OrchardFeature("Facebook")]
    public class FacebookServiceNotificationProvider: INotificationProvider {
        private readonly IOAuthProviderFacebookAuthenticator _oAuthProviderFacebookAuthenticator;

        public FacebookServiceNotificationProvider(IOAuthProviderFacebookAuthenticator oAuthProviderFacebookAuthenticator) {
            _oAuthProviderFacebookAuthenticator = oAuthProviderFacebookAuthenticator;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IEnumerable<NotifyEntry> GetNotifications() {
            if (!_oAuthProviderFacebookAuthenticator.IsConsumerConfigured) {
                yield return new NotifyEntry { Message = T("You need to add ClientKeyIdentifier and ClientSecret to enable Facebook feature."), Type = NotifyType.Warning };
            }
        }
    }
}
