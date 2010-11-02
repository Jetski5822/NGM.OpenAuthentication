using DotNetOpenAuth.OpenId;

namespace NGM.OpenAuthentication.Core {
    public sealed class OpenIdIdentifier {
        private OpenIdIdentifier() { }

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
            if (Identifier != null)
                return Identifier.ToString();
            return null;
        }
    }
}