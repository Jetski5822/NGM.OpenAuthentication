using System.Collections.Generic;
using NGM.OpenAuthentication.Core.Claims;
using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.Core.OpenId {
    public static class RegisterModelHelper {
        public static void PopulateModel(IList<UserClaims> claims, RegisterModel registerModel) {
            if (claims == null)
                return;

            foreach (var claim in claims) {
                if (string.IsNullOrEmpty(registerModel.Email)) {
                    registerModel.Email = claim.Contact.Email;
                    registerModel.UserName = claim.Contact.Email;
                }
            }
        }
    }
}