using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FilmServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FilmServiceSqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
