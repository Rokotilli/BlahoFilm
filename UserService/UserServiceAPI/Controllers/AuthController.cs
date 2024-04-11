using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IDataProtectionProvider _protectionProvider;
        private readonly IJWTHelper _jwthelper;
        private readonly UserServiceDbContext _dbContext;

        public AuthController(
            IAuthService authService,
            IConfiguration configuration,
            IDataProtectionProvider dataProtectionProvider,
            UserServiceDbContext userServiceDbContext,
            IJWTHelper jwthelper)
        {
            _authService = authService;
            _configuration = configuration;
            _protectionProvider = dataProtectionProvider;
            _dbContext = userServiceDbContext;
            _jwthelper = jwthelper;
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

        [HttpGet("google")]
        public async Task<IActionResult> GetGoogle()
        {
            var host = HttpContext.Request.Host.Value;

            var props = new AuthenticationProperties
            {
                RedirectUri = $"https://{host}/api/auth/callback",

                Items =
                {
                    { "url", _configuration["Google:RedirectUrl"] },
                    { "scheme", "Google" }
                }
            };

            return Challenge(props, props.Items["scheme"]);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync("Cookies");
            await HttpContext.SignOutAsync("Cookies");

            if (!result.Succeeded)
            {
                return BadRequest("External authentication failed!");
            }

            var externalUserId = result.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var externalUserEmail = result.Principal.FindFirst(ClaimTypes.Email).Value;

            var provider = result.Properties.Items["scheme"];
            var redirectUrl = result.Properties.Items["url"];

            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == externalUserEmail);
            var ifUserIsExternal = await _dbContext.Users.AnyAsync(u => u.ExternalProvider == null && u.Email == externalUserEmail);

            if (existUser == null)
            {
                var resultReg = await _authService.AddUser(new UserModel { Email = externalUserEmail }, provider, externalUserId);
            }
            else if (ifUserIsExternal)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, externalUserId),
                    new Claim(ClaimTypes.Email, externalUserEmail),
                    new Claim(ClaimTypes.System, provider)
                };

                var newToken = await _jwthelper.GenerateJwtToken(claims);

                var protect = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
                var encryptedToken = protect.Protect(newToken);

                return Redirect($"{_configuration["RedirectUrlToMigrate"]}?token=" + encryptedToken);
            }

            var resultAuth = await _authService.Authenticate(new UserModel { Email = externalUserEmail });            

            setTokensCookie(resultAuth);

            return Redirect(redirectUrl);
        }

        [HttpPost("migrateuser")]
        public async Task<IActionResult> MigrateUser([FromQuery] string token)
        {
            var protector = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            var decryptedToken = protector.Unprotect(token);

            var validToken = _jwthelper.ValidateJwtToken(decryptedToken);

            if (validToken == null)
            {
                return BadRequest("Invalid token!");
            }

            var email = validToken.Claims.First(t => t.Type == "email").Value;
            var externalId = validToken.Claims.First(t => t.Type == "nameid").Value;
            var provider = validToken.Claims.First(t => t.Type == ClaimTypes.System).Value;

            await _authService.MigrateUser(email, externalId, provider);

            var resultAuth = await _authService.Authenticate(new UserModel { Email = email });

            setTokensCookie(resultAuth);

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
            var encryptedAccessToken = protect.Protect(tokens.JwtToken);
            var encryptedRefreshToken = protect.Protect(tokens.RefreshToken);

            Response.Cookies.Append("accessToken", encryptedAccessToken, cookieAccessTokenOptions);
            Response.Cookies.Append("refreshToken", encryptedRefreshToken, cookieRefreshTokenOptions);
        }     
    }
}
