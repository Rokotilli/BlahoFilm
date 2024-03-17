using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserServiceDbContext _dbContext;

        public UsersService(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task<string> ChangeAvatar(string userid, IFormFile avatar)
        {
            try
            {
                byte[] avatarBytes = null;

                using (var stream = new MemoryStream())
                {
                    await avatar.CopyToAsync(stream);
                    avatarBytes = stream.ToArray();
                }

                var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userid);

                user.Avatar = avatarBytes;

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> ChangeTotalTime(string userid, int timeInSeconds)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userid);

                var totalSeconds = TimeSpan.Parse(user.TotalTime).TotalSeconds + timeInSeconds;

                user.TotalTime = TimeSpan.FromSeconds(totalSeconds).ToString();

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }   
    }
}
