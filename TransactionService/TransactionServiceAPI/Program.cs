using DataAccessLayer.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TransactionServiceAPI.Consumers;
using TransactionServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMyServices();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<TransactionServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TransactionServiceSqlServer"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddMassTransit(x =>
{
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

app.UseCors("Default");

app.MapControllers();

app.Run();
