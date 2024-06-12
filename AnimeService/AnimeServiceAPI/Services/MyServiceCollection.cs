using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

public static class MyServiceCollection
{
    public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IGetSaSService, GetSaSService>();
        serviceCollection.AddScoped<IAnimeService, AnimeService>();
        serviceCollection.AddScoped<IRegisterAnimeService, RegisterAnimeService>();
        serviceCollection.AddScoped<IUploadedAnimeService, UploadedAnimeService>();
        serviceCollection.AddScoped<ICommentService, CommentService>();
        serviceCollection.AddScoped<IRatingService, RatingService>();
        serviceCollection.AddScoped<IEncryptionHelper, EncryptionHelper>();
        serviceCollection.AddHostedService<OutboxMessagesBackgroundService>();
        
        return serviceCollection;
    }
}