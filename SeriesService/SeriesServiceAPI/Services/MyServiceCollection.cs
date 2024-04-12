using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

public static class MyServiceCollection
{
    public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IGetSaSService, GetSaSService>();
        serviceCollection.AddScoped<IRegisterSeriesService, RegisterSeriesService>();
        serviceCollection.AddScoped<IUploadedSeriesPartService, UploadedSeriesPartService>();
        serviceCollection.AddScoped<ICommentService, CommentService>();
        serviceCollection.AddScoped<IRatingService, RatingService>();

        return serviceCollection;
    }
}