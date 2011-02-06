using DotNetOpenAuth.OpenId;

namespace NGM.OpenAuthentication.Core.OpenId {
    public sealed class OpenIdIdentifier {
        public OpenIdIdentifier(string identifierString) {
            Identifier id;
            if (Identifier.TryParse(identifierString, out id)) {
                Identifier = id;
            }
        }

        public Identifier Identifier {
            get;
            private set;
        }

        public bool IsValid {
            get {
                return Identifier != null;
            }
        }

        public override string ToString() {
            return Identifier != null ? Identifier.ToString() : string.Empty;
        }
    }
}