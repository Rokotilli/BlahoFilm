using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace FilmServiceAPI.Services
{
    public static class MyServiceCollection
    {
        public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IGetSaSService, GetSaSService>();
            serviceCollection.AddScoped<IRegisterFilmService, RegisterFilmService>();
            serviceCollection.AddScoped<IUploadedFilmService, UploadedFilmService>();
            serviceCollection.AddScoped<ICommentService, CommentService>();
            serviceCollection.AddScoped<IRatingService, RatingService>();

            return serviceCollection;
        }
    }
}
