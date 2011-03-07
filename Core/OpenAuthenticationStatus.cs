namespace NGM.OpenAuthentication.Core {
    public enum OpenAuthenticationStatus {
        Unknown,

        ErrorAuthenticating,

        Authenticated,

        RequiresRegistration,

        RequresRedirect,

        AssociateOnLogon
    }
}