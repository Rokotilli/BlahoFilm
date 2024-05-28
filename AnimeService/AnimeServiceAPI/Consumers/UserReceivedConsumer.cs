using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;

namespace AnimeServiceAPI.Consumers
{
    public class UserReceivedConsumer : IConsumer<UserReceivedMessage>
    {
        private readonly AnimeServiceDbContext _dbContext;

        public UserReceivedConsumer(AnimeServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task Consume(ConsumeContext<UserReceivedMessage> consumeContext)
        {
            var model = _dbContext.Users.FirstOrDefault(u => u.UserId == consumeContext.Message.Id);

            if (model == null)
            {
                var user = new User()
                {
                    UserId = consumeContext.Message.Id,
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}