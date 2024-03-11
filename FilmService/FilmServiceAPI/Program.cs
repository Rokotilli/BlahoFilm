using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IGetSaSService, GetSaSService>();
builder.Services.AddScoped<IRegisterFilmService, RegisterFilmService>();
builder.Services.AddScoped<IUploadedFilmService, UploadedFilmService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddDbContext<FilmServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FilmServiceSqlServer"));
}, ServiceLifetime.Scoped);

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
