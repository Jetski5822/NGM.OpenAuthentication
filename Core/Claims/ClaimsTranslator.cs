namespace NGM.OpenAuthentication.Core.Claims {
    public interface IClaimsTranslator<in T> {
        UserClaims Translate(T response);
    }
}