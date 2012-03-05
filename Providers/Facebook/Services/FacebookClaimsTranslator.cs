using System.Collections.Generic;
using NGM.OpenAuthentication.Core.Claims;
using Orchard.Environment.Extensions;

namespace NGM.OpenAuthentication.Providers.Facebook.Services {
    [OrchardFeature("Facebook")]
    public class FacebookClaimsTranslator : IClaimsTranslator<IDictionary<string, object>> {
        public UserClaims Translate(IDictionary<string, object> response) {
            UserClaims claims = new UserClaims();

            claims.Contact = new ContactClaims();
            if (response.ContainsKey("email"))
                claims.Contact.Email = response["email"].ToString();

            return claims;
        }
    }
}