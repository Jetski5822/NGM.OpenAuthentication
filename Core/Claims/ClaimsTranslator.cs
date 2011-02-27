namespace NGM.OpenAuthentication.Core.Claims {
    public interface IClaimsTranslator<T> {
        UserClaims Translate(T response);
    }
}