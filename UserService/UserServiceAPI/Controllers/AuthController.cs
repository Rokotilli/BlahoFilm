using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IDataProtectionProvider _protectionProvider;

        public AuthController(IAuthService authService, IConfiguration configuration, IDataProtectionProvider dataProtectionProvider)
        {
            _authService = authService;
            _configuration = configuration;
            _protectionProvider = dataProtectionProvider;
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

            return Ok(result);
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

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> RemoveAllTokens()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok();
        }

        private void setTokensCookie(AuthResponse tokens)
        {
            var cookieAccessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var cookieRefreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
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
