using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;

namespace FilmServiceAPI.Consumers
{
    public class UserReceivedConsumer : IConsumer<UserReceivedMessage>
    {
        private readonly FilmServiceDbContext _dbContext;

        public UserReceivedConsumer(FilmServiceDbContext userServiceDbContext)
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