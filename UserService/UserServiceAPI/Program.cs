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
using UserServiceAPI.Consumers;
using UserServiceAPI.Services;

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

builder.Services.AddHttpClient("google", opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["OAuthGoogleApi"]);
});

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration["Redis:Host"];
    opt.InstanceName = builder.Configuration["Redis:InstanceName"];
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

builder.Services.AddDbContext<UserServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:UserServiceSqlServer"]);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = false,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ClockSkew = TimeSpan.Zero,
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
    x.AddConsumer<PremiumRemovedConsumer>();
    x.AddConsumer<PremiumReceivedConsumer>();
    x.AddConsumer<MediaRegisteredConsumer>();
    x.UsingRabbitMq((cxt, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMqHost"], "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.Publish<UserReceivedMessage>(publishTopology =>
        {
            publishTopology.ExchangeType = ExchangeType.Fanout;
            publishTopology.Durable = true;
        });
        cfg.ReceiveEndpoint("media-registered-queue-user-service", e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind<MediaRegisteredMessage>(b =>
            {
                b.ExchangeType = ExchangeType.Fanout;
            });
            e.ConfigureConsumer<MediaRegisteredConsumer>(cxt);
        });
        cfg.ReceiveEndpoint("premium-removed-queue-user-service", e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind<PremiumRemovedMessage>(b =>
            {
                b.ExchangeType = ExchangeType.Fanout;
            });
            e.ConfigureConsumer<PremiumRemovedConsumer>(cxt);
        });
        cfg.ReceiveEndpoint("premium-received-queue-user-service", e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind<PremiumReceivedMessage>(b =>
            {
                b.ExchangeType = ExchangeType.Fanout;
            });
            e.ConfigureConsumer<PremiumReceivedConsumer>(cxt);
        });
        cfg.ConfigureEndpoints(cxt);
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<UserServiceDbContext>().Database.Migrate();
}

app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
