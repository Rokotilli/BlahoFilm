using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUsersService
    {
        Task<string> ChangeAvatar(string userid, IFormFile avatar);
        Task<string> ChangeNickName(string userid, string username);
    }
}
