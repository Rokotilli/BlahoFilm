using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CartoonServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CartoonServiceSqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
