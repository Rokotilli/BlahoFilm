using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Options;
using Microsoft.Extensions.Options;

namespace BusinessLogicLayer.Services
{
    public class JWTHelper : IJWTHelper
    {
        private readonly AppSettings _appSettings;

        public JWTHelper(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }        

        public async Task<string> GenerateJwtToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Security.JwtSecretKey);            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Security.JwtIssuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken? ValidateJwtToken(string token)
        {
            if (token == null)
            {
                return null;
            }                

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _appSettings.Security.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Security.JwtSecretKey)),
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken;
            }
            catch
            {
                return null;
            }
        }
    }
}
