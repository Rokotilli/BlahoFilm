﻿using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.AdditionalModels;
using BusinessLogicLayer.Models.Enums;
using BusinessLogicLayer.Options;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace UserServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IUsersService _usersService;
        private readonly IDistributedCache _distributedCache;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        private readonly IEncryptionHelper _encryptionHelper;

        public UsersController(
            UserServiceDbContext userServiceDbContext,
            IUsersService usersService,
            IDistributedCache distributedCache,
            IOptions<AppSettings> options,
            IEmailService emailService,
            IEncryptionHelper encryptionHelper)
        {
            _dbContext = userServiceDbContext;
            _usersService = usersService;
            _distributedCache = distributedCache;
            _appSettings = options.Value;
            _emailService = emailService;
            _encryptionHelper = encryptionHelper;
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetUserById([FromQuery] string? id)
        {
            var userId = id ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await _dbContext.Users.Select(u => new
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                Avatar = u.Avatar,
                RegisterDate = u.RegisterDate,
                Roles = u.UserRoles.Select(ur => new Role { Id = ur.RoleId, Name = ur.Role.Name})
            }).FirstOrDefaultAsync(u => u.Id == userId);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("byids")]
        public async Task<IActionResult> GetUsersByIds([FromBody] string[] ids)
        {
            var model = _dbContext.Users
                .Where(u => ids
                .Contains(u.Id)).Select(u => new
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Avatar = u.Avatar,
                    RegisterDate = u.RegisterDate,
                    Roles = u.UserRoles.Select(ur => new Role { Id = ur.RoleId, Name = ur.Role.Name })
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("sendemailchangepassword")]
        public async Task<IActionResult> ChangePasswordCreateToken([FromForm] string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found!");
            }

            if (user.ExternalId != null)
            {
                return BadRequest("Account is managed by Google!");
            }

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var encryptedToken = _encryptionHelper.Encrypt(token);

            await _distributedCache.SetStringAsync(token, user.Email, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            await _emailService.SendEmailAsync(user.Email, _appSettings.RedirectUrlToChangePassword + "?token=" + encryptedToken, SendEmailActions.ChangePassword);

            return Ok();
        }

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] string token, [FromForm]
                                                                                  [RegularExpression(@"^(?=.*\d)(?=.*[A-Z]).+$", ErrorMessage = "Password must contain at least one uppercase letter and a number")]
                                                                                  [StringLength(24, MinimumLength = 8, ErrorMessage = "Max length 24 characters, min length 8 characters")]
                                                                                  string password)
        {
            string decryptedToken;
            try
            {
                decryptedToken = _encryptionHelper.Decrypt(token);
            }
            catch
            {
                return BadRequest("Invalid payload!");
            }

            var email = await _distributedCache.GetStringAsync(decryptedToken);

            if (email == null)
            {
                return BadRequest("Invalid token!");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found!");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

            user.PasswordHash = passwordHash;
            await _dbContext.SaveChangesAsync();

            await _distributedCache.RemoveAsync(token);

            return Ok();
        }

        [Authorize]
        [HttpPut("avatar")]
        public async Task<IActionResult> ChangeAvatar(IFormFile avatar)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _usersService.ChangeAvatar(userId, avatar);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("changenusername")]
        public async Task<IActionResult> ChangeNickName([FromQuery]
                                                        [StringLength(20, MinimumLength = 3, ErrorMessage = "Max length 20 characters, min length 3 characters")]
                                                        string username)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _usersService.ChangeNickName(userId, username);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("sendemailchangeemailaddress")]
        public async Task<IActionResult> ChangeEmailCreateToken(UserModel userModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var existEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email).Select(u => u.Email);

            if (existEmail != null)
            {
                return BadRequest("This email has already taken!");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user.ExternalId != null)
            {
                return BadRequest("Account is managed by Google!");
            }

            if (!BCrypt.Net.BCrypt.Verify(userModel.Password, user.PasswordHash))
            {
                return BadRequest("Password is incorrect!");
            }            

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var encryptToken = _encryptionHelper.Encrypt(token);

            await _distributedCache.SetStringAsync(token, JsonSerializer.Serialize(new ChangeEmailModel { UserEmail = user.Email, NewEmail = userModel.Email }), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            await _emailService.SendEmailAsync(userModel.Email, _appSettings.RedirectUrlToConfirmChangingEmail + "?token=" + encryptToken, SendEmailActions.ConfirmEmail);

            return Ok();
        }

        [HttpPut("changeemail")]
        public async Task<IActionResult> ChangeEmail([FromQuery] string token)
        {
            string decryptedToken;
            try
            {
                decryptedToken = _encryptionHelper.Decrypt(token);
            }
            catch
            {
                return BadRequest("Invalid payload!");
            }

            var json = await _distributedCache.GetStringAsync(decryptedToken);

            if (json == null)
            {
                return BadRequest("Invalid token!");
            }

            var model = JsonSerializer.Deserialize<ChangeEmailModel>(json);            

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            user.Email = model.NewEmail;
            await _dbContext.SaveChangesAsync();

            await _distributedCache.RemoveAsync(token);

            return Ok();
        }
    }
}
