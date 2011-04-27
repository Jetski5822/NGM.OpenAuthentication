using System;

namespace NGM.OpenAuthentication.Core {
    public enum Provider {
        OpenId,
        Twitter,
        Facebook,
        LiveId,
        CardSpace
    }

    public static class ProviderHelpers {
        public static int GetHashedProvider(Provider provider) {
            return provider.ToString().GetHashCode();
        }

        public static bool IsHashedProvider(int hashedProvider, Provider provider) {
            return hashedProvider == ProviderHelpers.GetHashedProvider(provider);
        }

        public static Provider GetProviderForHashedProvider(int hashedProvider) {
            if (IsHashedProvider(hashedProvider, Provider.Facebook)) {
                return Provider.Facebook;
            }
            if (IsHashedProvider(hashedProvider, Provider.LiveId)) {
                return Provider.LiveId;
            }
            if (IsHashedProvider(hashedProvider, Provider.Twitter)) {
                return Provider.Twitter;
            }
            if (IsHashedProvider(hashedProvider, Provider.OpenId)) {
                return Provider.OpenId;
            }
            throw new ArgumentOutOfRangeException();
        }

        public static string GetUserFriendlyStringForHashedProvider(int hashedProvider) {
            if (IsHashedProvider(hashedProvider, Provider.Facebook)) {
                return "Facebook";
            }
            if (IsHashedProvider(hashedProvider, Provider.LiveId)) {
                return "Microsoft Connect";
            }
            if (IsHashedProvider(hashedProvider, Provider.Twitter)) {
                return "Twitter";
            }
            if (IsHashedProvider(hashedProvider, Provider.OpenId)) {
                return "Open Id";
            }
            return "Unknown Provider";
        }
    }
}