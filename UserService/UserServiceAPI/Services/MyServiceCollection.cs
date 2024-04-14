﻿using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace UserServiceAPI.Services
{
    public static class MyServiceCollection
    {
        public static IServiceCollection AddMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IHistoryService, HistoryService>();
            serviceCollection.AddScoped<IUsersService, UsersService>();
            serviceCollection.AddScoped<IBookMarksService, BookMarksService>();
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IEmailService, EmailService>();
            serviceCollection.AddScoped<IJWTHelper, JWTHelper>();

            return serviceCollection;
        }
    }
}
