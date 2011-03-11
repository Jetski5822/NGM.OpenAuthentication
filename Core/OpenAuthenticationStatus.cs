namespace NGM.OpenAuthentication.Core {
    public enum OpenAuthenticationStatus {
        Unknown,

        ErrorAuthenticating,

        Authenticated,

        RequresRedirect,

        AssociateOnLogon,
        
        UserDoesNotExist
    }
}