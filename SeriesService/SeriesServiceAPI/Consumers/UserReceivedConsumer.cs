using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;

namespace SeriesServiceAPI.Consumers
{
    public class UserReceivedConsumer : IConsumer<UserReceivedMessage>
    {
        private readonly SeriesServiceDbContext _dbContext;

        public UserReceivedConsumer(SeriesServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task Consume(ConsumeContext<UserReceivedMessage> consumeContext)
        {
            var model = new User()
            {
                UserId = consumeContext.Message.Id
            };

            _dbContext.Users.Add(model); 
            await _dbContext.SaveChangesAsync();
        }
    }
}