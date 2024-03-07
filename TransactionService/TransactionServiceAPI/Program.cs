using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransactionServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TransactionServiceSqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
