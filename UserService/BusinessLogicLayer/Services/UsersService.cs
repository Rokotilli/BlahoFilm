using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userid);

                user.Avatar = avatarBytes;

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Changing avatar failed!";
            }
        }

        public async Task<string> ChangeNickName(string userid, string username)
        {
            try
            {
                var existUserName = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

                if (existUserName != null)
                {
                    return "This user name has already taken!";
                }

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userid);

                user.UserName = username;

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Changing username failed!";
            }
        }
    }
}
