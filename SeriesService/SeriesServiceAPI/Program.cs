using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SeriesServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SeriesServiceSqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
