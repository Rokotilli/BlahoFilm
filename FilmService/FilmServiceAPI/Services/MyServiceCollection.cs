using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace FilmServiceAPI.Services
{
    public static class MyServiceCollection
    {
        public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IGetSaSService, GetSaSService>();
            serviceCollection.AddScoped<IFilmService, FilmService>();
            serviceCollection.AddScoped<IUploadedVoiceoverService, UploadedVoiceoverService>();
            serviceCollection.AddScoped<ICommentService, CommentService>();
            serviceCollection.AddScoped<IRatingService, RatingService>();

            return serviceCollection;
        }
    }
}
