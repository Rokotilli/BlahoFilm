﻿using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Enums;
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
                    var dbContext = scope.ServiceProvider.GetRequiredService<FilmServiceDbContext>();
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                    var notpublishedMessages = dbContext.OutboxMessages.Where(m => !m.IsPublished).ToList();

                    foreach (var item in notpublishedMessages)
                    {
                        var film = item.Data as Film;

                        try
                        {
                            await publishEndpoint.Publish(new MediaRegisteredMessage() { Id = film.Id, MediaType = MediaTypes.Film }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);
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