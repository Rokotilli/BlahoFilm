using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.Services
{
    public class OutboxMessagesBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OutboxMessagesBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TransactionServiceDbContext>();
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                    var notpublishedMessages = dbContext.OutboxMessages.Where(m => !m.IsPublished).ToList();

                    foreach (var item in notpublishedMessages)
                    {
                        var subsc = item.Data as Subscription;

                        try
                        {
                            await publishEndpoint.Publish(!subsc.IsActive ? new PremiumRemovedMessage { UserId = subsc.UserId } : new PremiumReceivedMessage { UserId = subsc.UserId }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);

                            item.IsPublished = true;

                            await dbContext.SaveChangesAsync();
                        }
                        catch { continue; }
                    }
                }

                await Task.Delay(10000 * 60, stoppingToken);
            }
        }
    }
}
