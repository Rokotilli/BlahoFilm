using MassTransit;
using MessageBus.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class MessagesBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessagesBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                                        
                    var notpublishedMessages = dbContext.OutboxMessages.Where(m => !m.IsPublished).ToList();

                    foreach (var item in notpublishedMessages)
                    {
                        var user = item.Data as User;

                        try
                        {
                            await publishEndpoint.Publish(new UserReceivedMessage { Id = user.Id }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);
                        }
                        catch { break; }                        

                        item.IsPublished = true;

                        await dbContext.SaveChangesAsync();
                    }                   
                }

                await Task.Delay(10000 * 60, stoppingToken);
            }
        }
    }
}
