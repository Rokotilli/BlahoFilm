using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;

namespace UserServiceAPI.Consumers
{
    public class UserReceivedConsumer : IConsumer<UserReceivedMessage>
    {
        private readonly UserServiceDbContext _dbContext;

        public UserReceivedConsumer(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task Consume(ConsumeContext<UserReceivedMessage> consumeContext)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == consumeContext.Message.Id);

            if (user == null)
            {
                var model = new User()
                {
                    UserId = consumeContext.Message.Id,
                    UserName = consumeContext.Message.UserName
                };

                _dbContext.Users.Add(model);
                await _dbContext.SaveChangesAsync();

                return;
            }   

            user.UserName = consumeContext.Message.UserName;

            await _dbContext.SaveChangesAsync();
        }
    }
}
