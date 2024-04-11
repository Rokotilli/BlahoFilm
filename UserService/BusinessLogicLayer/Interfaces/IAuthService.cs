using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        Task<string> AddUser(UserModel addUserModel, string externalProvider = null, string externalId = null);
        Task<AuthResponse> Authenticate(UserModel userModel);
        Task<AuthResponse> RefreshJwtToken(string token);
        Task MigrateUser(string email, string externalId, string externalProvider);
    }
}
