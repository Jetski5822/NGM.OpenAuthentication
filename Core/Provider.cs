using System;
using System.Security.Cryptography;
using System.Text;

namespace NGM.OpenAuthentication.Core {
    public enum Provider {
        OpenId,
        Twitter,
        Facebook,
        LiveId,
        CardSpace
    }

    public static class ProviderHelpers {
        public static string GetHashedProvider(Provider provider) {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(provider.ToString()));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static bool IsHashedProvider(string hashedProvider, Provider provider) {
            return hashedProvider == ProviderHelpers.GetHashedProvider(provider);
        }

        public static Provider GetProviderForHashedProvider(string hashedProvider) {
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

        public static string GetUserFriendlyStringForHashedProvider(string hashedProvider) {
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