using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Options;
using DataAccessLayer.Context;
using MassTransit;
using MessageBus.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Text;
using TransactionServiceAPI.Consumers;
using TransactionServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddAzureAppConfiguration(config =>
    {
        config.Connect(builder.Configuration["ConnectionStrings:AzureAppConfiguration"]);
    });
}

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddMyServices();

builder.Services.AddHttpClient("paypal", opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["PayPalConfigs:Url"]);
});

builder.Services.AddDbContext<TransactionServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:TransactionServiceSqlServer"]);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", opt =>
    {
        var origins = builder.Configuration["Security:AllowedOrigins"].Split(",");
        opt.WithOrigins(origins)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = false,
                           ValidateLifetime = true,
                           ClockSkew = TimeSpan.Zero,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = builder.Configuration["Security:JwtIssuer"],
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Security:JwtSecretKey"]))
                       };
                       options.Events = new JwtBearerEvents
                       {
                           OnMessageReceived = context =>
                           {
                               var encryptionHelper = context.HttpContext.RequestServices.GetRequiredService<IEncryptionHelper>();
                               var encryptedToken = context.Request.Cookies["accessToken"];

                               if (!string.IsNullOrEmpty(encryptedToken))
                               {
                                   try
                                   {
                                       var decryptedToken = encryptionHelper.Decrypt(encryptedToken);
                                       context.Token = decryptedToken;
                                   }
                                   catch { };
                               }
                               return Task.CompletedTask;
                           }
                       };
                   });

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserReceivedConsumer>();
    x.UsingRabbitMq((cxt, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMqHost"], "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.Publish<PremiumReceivedMessage>(publishTopology =>
        {
            publishTopology.ExchangeType = ExchangeType.Fanout;
            publishTopology.Durable = true;
        });
        cfg.Publish<PremiumRemovedMessage>(publishTopology =>
        {
            publishTopology.ExchangeType = ExchangeType.Fanout;
            publishTopology.Durable = true;
        });
        cfg.ReceiveEndpoint("user-received-queue-transaction-service", e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind<UserReceivedMessage>(b =>
            {
                b.ExchangeType = ExchangeType.Fanout;
            });
            e.ConfigureConsumer<UserReceivedConsumer>(cxt);
        });
        cfg.ConfigureEndpoints(cxt);
    });
});

var app = builder.Build();

app.UseCors("AllowOrigin");

if (!app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<TransactionServiceDbContext>().Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
