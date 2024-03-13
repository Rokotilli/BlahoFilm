using DataAccessLayer.Context;
using FilmServiceAPI.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMyServices();

builder.Services.AddDbContext<FilmServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FilmServiceSqlServer"));
});

builder.Services.AddMassTransit(x =>
{
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

app.MapControllers();

app.Run();
