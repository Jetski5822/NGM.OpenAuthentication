namespace NGM.OpenAuthentication.Core.OpenId {
    public sealed class CardSpaceAuthenticationParameters : OpenAuthenticationParameters {
        public override string Provider {
            get { return "CardSpace"; }
        }
    }
}