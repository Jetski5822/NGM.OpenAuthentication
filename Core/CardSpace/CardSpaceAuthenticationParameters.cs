namespace NGM.OpenAuthentication.Core.CardSpace {
    public sealed class CardSpaceAuthenticationParameters : OpenAuthenticationParameters {
        public override string Provider {
            get { return "CardSpace"; }
        }
    }
}