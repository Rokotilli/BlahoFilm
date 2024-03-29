using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace TransactionServiceAPI.Services
{
    public static class MyServiceCollection
    {
        public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFundraisingService, FundraisingService>();
            serviceCollection.AddScoped<ITransactionService, TransactionService>();
            serviceCollection.AddHostedService<SubscriptionBackgroundService>();

            return serviceCollection;
        }
    }
}
