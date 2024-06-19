using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Enums;
using BusinessLogicLayer.Options;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MessageBus.Messages;
using MessageBus.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BusinessLogicLayer.Services
{
    public class AuthResponse
    {
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Exception { get; set; }
    }

    public class RegisterResponse
    {
        public User User { get; set; }
        public string? Exception { get; set; }
    }

    public class AuthService : IAuthService
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly AppSettings _appSettings;
        private readonly IJWTHelper _jWTHelper;
        private readonly IEmailService _emailService;
        private readonly IEncryptionHelper _encryptionHelper;

        public AuthService(
            UserServiceDbContext userServiceDbContext,
            IPublishEndpoint publishEndpoint,
            IOptions<AppSettings> options,
            IJWTHelper jWTHelper,
            IEmailService emailService,
            IEncryptionHelper encryptionHelper)
        {
            _dbContext = userServiceDbContext;
            _publishEndpoint = publishEndpoint;
            _appSettings = options.Value;
            _jWTHelper = jWTHelper;
            _emailService = emailService;
            _encryptionHelper = encryptionHelper;
        }

        public async Task<RegisterResponse> AddUser(UserModel userModel, string externalProvider = null, string externalId = null)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);

                if (user != null && user.Email == userModel.Email)
                {
                    return new RegisterResponse { Exception = "This email has already taken!" };
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(userModel.Password ?? "", BCrypt.Net.BCrypt.GenerateSalt());                

                var addedUser = await _dbContext.Users.AddAsync(new User()
                {
                    ExternalId = externalId,
                    ExternalProvider = externalProvider,
                    UserName = getUniqueUserName(),
                    Email = userModel.Email,
                    EmailConfirmed = externalProvider == null ? false : true,
                    PasswordHash = externalProvider == null ? passwordHash : null
                });

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userModel.Email)
                };

                if (externalProvider == null)
                {
                    var token = await _jWTHelper.GenerateJwtToken(claims);
                    var encryptedToken = _encryptionHelper.Encrypt(token);
                    await _emailService.SendEmailAsync(userModel.Email, _appSettings.RedirectUrlToConfirmEmail + "?token=" + encryptedToken, SendEmailActions.ConfirmEmail);
                }

                await _dbContext.UserRoles.AddAsync(new UserRole() { UserId = addedUser.Entity.Id, RoleId = 1 });

                var addedMessage = await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(addedUser.Entity));

                try
                {
                    await _publishEndpoint.Publish(new UserReceivedMessage() { Id = addedUser.Entity.Id }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);

                    addedMessage.Entity.IsPublished = true;
                }
                catch { }

                await _dbContext.SaveChangesAsync();

                return new RegisterResponse { User = addedUser.Entity };
            }
            catch (Exception ex)
            {
                return new RegisterResponse { Exception = "Adding user failed!" };
            }            
        }

        public async Task<AuthResponse> Authenticate(User user)
        {
            try
            {               
                var expiredRefreshTokens = _dbContext.RefreshTokens
                                            .Where(rt => rt.UserId == user.Id && rt.Expires < DateTime.UtcNow)
                                            .ToArray();

                if (expiredRefreshTokens.Any())
                {
                    _dbContext.RefreshTokens.RemoveRange(expiredRefreshTokens);
                }

                var userRoles = _dbContext.UserRoles
                                    .Include(ur => ur.Role)
                                    .Where(ur => ur.UserId == user.Id)
                                    .Select(ur => ur.Role.Name)
                                    .ToArray();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var jwtToken = await _jWTHelper.GenerateJwtToken(claims);
                var refrestToken = await GenerateRefreshToken(user.Id);

                await _dbContext.RefreshTokens.AddAsync(refrestToken);
                await _dbContext.SaveChangesAsync();

                return new AuthResponse() { JwtToken = jwtToken, RefreshToken = refrestToken.Token };
            }
            catch (Exception ex)
            {
                return new AuthResponse() { Exception = "Authentication user failed!" };
            }
        }

        public async Task<AuthResponse> RefreshJwtToken(string token)
        {
            try
            {                
                var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);

                if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow)
                {
                    if (refreshToken != null)
                    {
                        _dbContext.RefreshTokens.Remove(refreshToken);
                    }
                    return new AuthResponse() { Exception = "Invalid token!" };
                }

                var user = await GetUserByRefreshToken(token);

                var userRoles = _dbContext.UserRoles
                    .Include(ur => ur.Role)
                    .Where(ur => ur.UserId == user.Id)
                    .Select(ur => ur.Role.Name)
                    .ToArray();

                var newRefreshToken = await GenerateRefreshToken(user.Id);
                await _dbContext.RefreshTokens.AddAsync(newRefreshToken);

                await _dbContext.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var jwtToken = await _jWTHelper.GenerateJwtToken(claims);

                return new AuthResponse { JwtToken = jwtToken, RefreshToken = newRefreshToken.Token };
            }
            catch (Exception ex)
            {
                return new AuthResponse() { Exception = "Refreshing JWT failed!" };
            }
        }

        public async Task<string> CheckUserEmailForMigrate(User user, string externalEmail, string externalId, string provider)
        {          
            if (user.ExternalId == null && user.Email == externalEmail)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, externalId),
                    new Claim(ClaimTypes.Email, externalEmail),
                    new Claim(ClaimTypes.System, provider)
                };

                var newToken = await _jWTHelper.GenerateJwtToken(claims);

                return newToken;
            }

            if (user.ExternalId == externalId && user.Email != externalEmail)
            {
                user.Email = externalEmail;
                await _dbContext.SaveChangesAsync();
            }            

            return null;
        }

        private string getUniqueUserName()
        {
            var randomUserName = "User" + (Random.Shared.Next(0, 900000000) + 100000000);

            var userNameIsUnique = !_dbContext.Users.Any(u => u.UserName == randomUserName);

            if (!userNameIsUnique)
            {
                return getUniqueUserName();
            }

            return randomUserName;
        }

        private async Task<RefreshToken> GenerateRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(int.Parse(_appSettings.Security.RefreshTokenTTL)),
                Created = DateTime.UtcNow                
            };

            return refreshToken;
        }

        private async Task<User> GetUserByRefreshToken(string token)
        {
            var user = await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token)
                .Select(u => u.User);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
