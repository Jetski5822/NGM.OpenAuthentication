namespace NGM.OpenAuthentication.Core.CardSpace {
    public sealed class CardSpaceAuthenticationParameters : OpenAuthenticationParameters {
        public override Provider Provider {
            get { return Provider.CardSpace; }
        }
    }
}