using DataAccessLayer.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Consumers;
using UserServiceAPI.Hubs;
using UserServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMyServices();

builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
});

builder.Services.AddDbContext<UserServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserServiceSqlServer"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MediaRegisteredConsumer>();
    x.AddConsumer<UserReceivedConsumer>();
    x.UsingRabbitMq((cxt, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("RabbitMqHost"), "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(cxt);
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<UserServiceDbContext>().Database.Migrate();
}

app.MapHub<WatchingFilmHub>("/watchingfilm");

app.MapControllers();

app.Run();
