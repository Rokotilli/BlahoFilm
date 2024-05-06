using DataAccessLayer.Context;
using FilmServiceAPI.Consumers;
using FilmServiceAPI.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddAzureAppConfiguration(config =>
    {
        config.Connect(builder.Configuration["ConnectionStrings:AzureAppConfiguration"]);
    });
}

builder.Services.AddControllers();

builder.Services.AddMyServices();

builder.Services.AddDataProtection(opt =>
{
    opt.ApplicationDiscriminator = builder.Configuration["Security:CookieProtectKey"];
});

builder.Services.AddDbContext<FilmServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:FilmServiceSqlServer"]);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", opt =>
    {
        var origins = builder.Configuration["AllowedOrigins"].Split(",");
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
                               var _dataProtectionProvider = context.HttpContext.RequestServices.GetRequiredService<IDataProtectionProvider>();
                               var encryptedToken = context.Request.Cookies["accessToken"];
                               var protector = _dataProtectionProvider.CreateProtector(builder.Configuration["Security:CookieProtectKey"]);

                               if (!string.IsNullOrEmpty(encryptedToken))
                               {
                                   try
                                   {
                                       var token = protector.Unprotect(encryptedToken);
                                       context.Token = token;
                                   }
                                   catch (Exception ex) 
                                   {
                                       Console.WriteLine(ex);
                                   };
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
        cfg.Host(builder.Configuration.GetValue<string>("RabbitMqHost"), "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(cxt);
    });
});

var app = builder.Build();

app.UseCors("AllowOrigin");

if (!app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<FilmServiceDbContext>().Database.Migrate();
}

app.MapControllers();

app.Run();
