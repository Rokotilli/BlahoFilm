using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        Task<string> AddUser(UserModel addUserModel);
        Task<AuthResponse> Authenticate(UserModel userModel);
        Task<AuthResponse> RefreshJwtToken(string token);
    }
}
