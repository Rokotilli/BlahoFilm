using Microsoft.IdentityModel.Tokens;

namespace FilmServiceAPI.JWTValidators
{
    public static class MyLifetimeValidator
    {
        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters parameters)
        {
            if (notBefore > DateTime.UtcNow || expires < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
