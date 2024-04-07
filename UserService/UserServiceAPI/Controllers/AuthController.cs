using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IDataProtectionProvider _protectionProvider;
        private readonly UserServiceDbContext _dbContext;

        public AuthController(IAuthService authService, IConfiguration configuration, IDataProtectionProvider dataProtectionProvider, UserServiceDbContext userServiceDbContext)
        {
            _authService = authService;
            _configuration = configuration;
            _protectionProvider = dataProtectionProvider;
            _dbContext = userServiceDbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser(UserModel addUserModel)
        {
            var result = await _authService.AddUser(addUserModel);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthUser(UserModel userModel)
        {
            var result = await _authService.Authenticate(userModel);

            if (result.Exception != null)
            {
                return BadRequest(result.Exception);
            }

            setTokensCookie(result);

            return Ok();
        }

        [HttpPut("refreshjwt")]
        public async Task<IActionResult> RefreshAccessToken()
        {
            var encryptedToken = HttpContext.Request.Cookies["refreshToken"];
            var protector = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            var token = protector.Unprotect(encryptedToken);

            var result = await _authService.RefreshJwtToken(token);

            if (result.Exception != null)
            {
                return BadRequest(result.Exception);
            }

            setTokensCookie(result);

            return Ok();
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> RemoveAllTokens()
        {
            var encryptedToken = HttpContext.Request.Cookies["refreshToken"];
            var protector = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            var token = protector.Unprotect(encryptedToken);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            var model = _dbContext.RefreshTokens.FirstOrDefault(rt => rt.Token == token && rt.UserId == userId);

            if (model == null)
            {
                return BadRequest("Refresh token not found!");
            }

            _dbContext.RefreshTokens.Remove(model);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        private void setTokensCookie(AuthResponse tokens)
        {
            var cookieAccessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var cookieRefreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["Security:RefreshTokenTTL"]))
            };

            var protect = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            var encryptedValueAccessToken = protect.Protect(tokens.JwtToken);
            var encryptedValueRefreshToken = protect.Protect(tokens.RefreshToken);

            Response.Cookies.Append("accessToken", encryptedValueAccessToken, cookieAccessTokenOptions);
            Response.Cookies.Append("refreshToken", encryptedValueRefreshToken, cookieRefreshTokenOptions);
        }
    }
}
