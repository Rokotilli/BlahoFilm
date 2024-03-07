using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserServiceSqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
