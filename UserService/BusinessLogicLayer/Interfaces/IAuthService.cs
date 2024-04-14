using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> AddUser(UserModel userModel, string externalProvider = null, string externalId = null);
        Task<AuthResponse> Authenticate(UserModel userModel);
        Task<AuthResponse> RefreshJwtToken(string token);
        Task<string> MigrateUser(string externalEmail, string externalId, string externalProvider);
        Task<string> CheckUserEmailForMigrate(User user, string externalEmail, string externalId, string provider);
    }
}
