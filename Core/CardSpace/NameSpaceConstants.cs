namespace NGM.OpenAuthentication.Core.CardSpace {
    public static class NamespaceConstants {
        public const string ClaimsNamespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";

        public static string BuildClaimFullNamespaceUrl(string claimValue) {
            var formattedClaimValue = claimValue;
            if (claimValue.StartsWith("/"))
                formattedClaimValue = claimValue.Remove(0, 1);

            return string.Format("{0}{1}", ClaimsNamespace, formattedClaimValue);
        }
    }
}