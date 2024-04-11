using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IJWTHelper
    {
        public Task<string> GenerateJwtToken(List<Claim> claims);
        public JwtSecurityToken? ValidateJwtToken(string token);
    }
}
