namespace NGM.OpenAuthentication.Core {
    public enum Statuses {
        Unknown,

        ErrorAuthenticating,

        Authenticated,

        RequiresRedirect,

        AssociateOnLogon,
        
        UserDoesNotExist
    }
}