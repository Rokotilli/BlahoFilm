﻿using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using BusinessLogicLayer.Models.Enums;
using MassTransit.Initializers;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IDataProtectionProvider _protectionProvider;
        private readonly IJWTHelper _jWTHelper;
        private readonly IEmailService _emailService;
        private readonly HttpClient _httpClient;
        private readonly UserServiceDbContext _dbContext;

        public AuthController(
            IAuthService authService,
            IConfiguration configuration,
            IDataProtectionProvider dataProtectionProvider,
            UserServiceDbContext userServiceDbContext,
            IJWTHelper jwthelper,
            IEmailService emailService,
            IHttpClientFactory httpClientFactory)
        {
            _authService = authService;
            _configuration = configuration;
            _protectionProvider = dataProtectionProvider;
            _dbContext = userServiceDbContext;
            _jWTHelper = jwthelper;
            _emailService = emailService;
            _httpClient = httpClientFactory.CreateClient("google");
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser(UserModel addUserModel)
        {            
            var result = await _authService.AddUser(addUserModel);

            if (result.Exception != null)
            {
                return BadRequest(result.Exception);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, addUserModel.Email)
            };

            var token = await _jWTHelper.GenerateJwtToken(claims);
            var protect = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            var encryptedToken = protect.Protect(token);
            await _emailService.SendEmailAsync(addUserModel.Email, _configuration["RedirectUrlToConfirmEmail"] + "?token=" + encryptedToken, SendEmailActions.ConfirmEmail);

            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthUser(UserModel userModel)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);

            if (user == null)
            {
                return NotFound("User not found!");
            }

            if (!user.EmailConfirmed)
            {
                return StatusCode(403, "Email is not confirmed!");
            }

            if (user.PasswordHash == null)
            {
                return BadRequest("User is registered through a remote provider");
            }

            if (!BCrypt.Net.BCrypt.Verify(userModel.Password, user.PasswordHash))
            {
                return BadRequest("Password is incorrect!");
            }

            var result = await _authService.Authenticate(user);

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
            string decryptedToken;
            try
            {
                decryptedToken = protector.Unprotect(encryptedToken);
            }
            catch
            {
                return BadRequest("Invalid payload!");
            }

            var result = await _authService.RefreshJwtToken(decryptedToken);

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var encryptedToken = HttpContext.Request.Cookies["refreshToken"];
            var protector = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            string decryptedToken;
            try
            {
                decryptedToken = protector.Unprotect(encryptedToken);
            }
            catch
            {
                return BadRequest("Invalid payload!");
            }            

            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            var model = _dbContext.RefreshTokens.FirstOrDefault(rt => rt.Token == decryptedToken && rt.UserId == userId);

            if (model == null)
            {
                return BadRequest("Refresh token not found!");
            }

            _dbContext.RefreshTokens.Remove(model);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("google")]
        public async Task<IActionResult> Callback([FromForm] string token)
        {
            var response = await _httpClient.GetAsync($"tokeninfo?access_token={token}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("External authentication failed!");
            }

            var result = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<dynamic>(result);            

            string externalUserId = payload.sub;
            string externalUserEmail = payload.email;

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == externalUserEmail || u.ExternalId == externalUserId);

            if (user == null)
            {
                user = _authService.AddUser(new UserModel { Email = externalUserEmail }, "Google", externalUserId).Result.User;
            }

            var resultToken = await _authService.CheckUserEmailForMigrate(user, externalUserEmail, externalUserId, "Google");

            if (resultToken != null)
            {
                var protect = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
                var encryptedToken = protect.Protect(resultToken);
                return Redirect($"{_configuration["RedirectUrlToMigrate"]}?token=" + encryptedToken);
            }

            var resultAuth = await _authService.Authenticate(user);

            setTokensCookie(resultAuth);

            return Ok();
        }

        [HttpPost("migrateuser")]
        public async Task<IActionResult> MigrateUser([FromQuery] string token)
        {
            var protector = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            string decryptedToken;
            try
            {
                decryptedToken = protector.Unprotect(token);
            }
            catch
            {
                return BadRequest("Invalid payload!");
            }

            var validToken = _jWTHelper.ValidateJwtToken(decryptedToken);

            if (validToken == null)
            {
                return BadRequest("Invalid token!");
            }

            var externalEmail = validToken.Claims.First(t => t.Type == "email").Value;
            var externalId = validToken.Claims.First(t => t.Type == "nameid").Value;
            var provider = validToken.Claims.First(t => t.Type == ClaimTypes.System).Value;

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == externalEmail);

            if (user == null)
            {
                return NotFound("User not found!");
            }

            if (user.ExternalProvider != null)
            {
                return Conflict("Account already migrated!");
            }

            user.ExternalProvider = provider;
            user.EmailConfirmed = true;
            user.ExternalId = externalId;
            user.PasswordHash = null;

            await _dbContext.SaveChangesAsync();

            var resultAuth = await _authService.Authenticate(user);

            setTokensCookie(resultAuth);

            return Ok();
        }

        [HttpPost("emailconfirm")]
        public async Task<IActionResult> EmailConfirm([FromQuery] string token)
        {
            var protector = _protectionProvider.CreateProtector(_configuration["Security:CookieProtectKey"]);
            string decryptedToken;
            try
            {
                decryptedToken = protector.Unprotect(token);
            }
            catch
            {
                return BadRequest("Invalid payload!");
            }

            var validToken = _jWTHelper.ValidateJwtToken(decryptedToken);

            if (validToken == null)
            {
                return BadRequest("Invalid token!");
            }

            var email = validToken.Claims.First(t => t.Type == "email").Value;

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email); 

            if (user.EmailConfirmed == true)
            {
                return Conflict("Email already confirmed!");
            }

            user.EmailConfirmed = true;
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
            var encryptedAccessToken = protect.Protect(tokens.JwtToken);
            var encryptedRefreshToken = protect.Protect(tokens.RefreshToken);

            Response.Cookies.Append("accessToken", encryptedAccessToken, cookieAccessTokenOptions);
            Response.Cookies.Append("refreshToken", encryptedRefreshToken, cookieRefreshTokenOptions);
        }     
    }
}
