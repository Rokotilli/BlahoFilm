using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using SeriesServiceAPI.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SeriesServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SeriesServiceSqlServer"));
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
    app.Services.GetRequiredService<SeriesServiceDbContext>().Database.Migrate();
}

app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();
 