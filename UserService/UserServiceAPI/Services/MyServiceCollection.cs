using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace UserServiceAPI.Services
{
    public static class MyServiceCollection
    {
        public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFavoritesService, FavoritesService>();

            return serviceCollection;
        }
    }
}
