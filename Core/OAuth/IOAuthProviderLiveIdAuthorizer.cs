namespace NGM.OpenAuthentication.Core.OAuth {
    public interface IOAuthProviderLiveIdAuthorizer : IOAuthProviderAuthorizer {
        void LogOut(string returnUrl);
    }
}