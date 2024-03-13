using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;

namespace UserServiceAPI.Consumers
{
    public class MediaRegisteredConsumer : IConsumer<MediaRegisteredMessage>
    {
        private readonly UserServiceDbContext _dbContext;

        public MediaRegisteredConsumer(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task Consume(ConsumeContext<MediaRegisteredMessage> consumeContext)
        {
            var model = new MediaWithType()
            {
                MediaId = consumeContext.Message.Id,
                MediaTypeId = ((int)consumeContext.Message.MediaType)
            };

            _dbContext.MediaWithTypes.Add(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
