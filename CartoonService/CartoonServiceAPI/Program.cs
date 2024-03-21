using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using CartoonServiceAPI.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CartoonServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CartoonServiceSqlServer"));
});
builder.Services.AddControllers();
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

if (!app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<CartoonServiceDbContext>().Database.Migrate();
}

app.MapControllers();
app.Run();
