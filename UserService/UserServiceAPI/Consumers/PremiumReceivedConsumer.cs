using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;

namespace UserServiceAPI.Consumers
{
    public class PremiumReceivedConsumer : IConsumer<PremiumReceivedMessage>
    {
        private readonly UserServiceDbContext _dbContext;

        public PremiumReceivedConsumer(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task Consume(ConsumeContext<PremiumReceivedMessage> consumeContext)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == consumeContext.Message.UserId);

            if (user != null)
            {
                var existPremium = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == 2);

                if (existPremium == null)
                {
                    await _dbContext.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = 2 });
                    await _dbContext.SaveChangesAsync();
                    return;
                }

                _dbContext.UserRoles.Remove(new UserRole { UserId = user.Id, RoleId = 2 });
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
