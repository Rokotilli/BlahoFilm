﻿using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

    public class AuthService : IAuthService
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfiguration _configuration;
        private readonly IJWTHelper _jWTHelper;

        public AuthService(UserServiceDbContext userServiceDbContext, IPublishEndpoint publishEndpoint, IConfiguration configuration, IJWTHelper jWTHelper)
        {
            _dbContext = userServiceDbContext;
            _publishEndpoint = publishEndpoint;
            _configuration = configuration;
            _jWTHelper = jWTHelper;
        }

        public async Task<string> AddUser(UserModel userModel, string externalProvider = null, string externalId = null)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);

                if (user != null && user.Email == userModel.Email)
                {
                    return "This email has already taken!";
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

                await _dbContext.UserRoles.AddAsync(new UserRole() { UserId = addedUser.Entity.Id, RoleId = 1 });
                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new UserReceivedMessage() { Id = addedUser.Entity.Id });

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            string getUniqueUserName()
            {
                var randomUserName = "User" + (Random.Shared.Next(0, 900000000) + 100000000);

                var userNameIsUnique = !_dbContext.Users.Any(u => u.UserName == randomUserName);

                if (!userNameIsUnique)
                {
                    return getUniqueUserName();
                }                   

                return randomUserName;
            }
        }

        public async Task<AuthResponse> Authenticate(UserModel userModel)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);

                if (user == null)
                {
                    return new AuthResponse() { Exception = "User not found!" };
                }

                if (userModel.Password != null && !BCrypt.Net.BCrypt.Verify(userModel.Password, user.PasswordHash))
                {
                    return new AuthResponse() { Exception = "Password is incorrect!" };
                }

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
                return new AuthResponse() { Exception = ex.ToString() };
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
                return new AuthResponse() { Exception = ex.ToString() };
            }
        }

        public async Task MigrateUser(string email, string externalId, string externalProvider)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            user.ExternalProvider = externalProvider;
            user.EmailConfirmed = true;
            user.ExternalId = externalId;
            user.PasswordHash = null;

            await _dbContext.SaveChangesAsync();
        }

        private async Task<RefreshToken> GenerateRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["Security:RefreshTokenTTL"])),
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
