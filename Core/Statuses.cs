namespace NGM.OpenAuthentication.Core {
    public enum Statuses {
        Unknown,

        ErrorAuthenticating,

        Authenticated,

        RequresRedirect,

        AssociateOnLogon,
        
        UserDoesNotExist
    }
}