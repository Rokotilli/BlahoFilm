using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IGetSaSService, GetSaSService>();
builder.Services.AddScoped<IRegisterFilmService, RegisterFilmService>();
builder.Services.AddScoped<IUploadedFilmService, UploadedFilmService>();

builder.Services.AddDbContext<FilmServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FilmServiceSqlServer"));
});

var app = builder.Build();

app.MapControllers();

app.Run();
