using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace TransactionServiceAPI.Services
{
    public static class MyServiceCollection
    {
        public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFundraisingService, FundraisingService>();
            serviceCollection.AddScoped<ISubscriptionService, SubscriptionService>();
            serviceCollection.AddHostedService<SubscriptionBackgroundService>();
            serviceCollection.AddHostedService<OutboxMessagesBackgroundService>();
            serviceCollection.AddScoped<IEncryptionHelper, EncryptionHelper>();

            return serviceCollection;
        }
    }
}
