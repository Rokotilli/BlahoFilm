using AnimeServiceAPI.Consumers;
using DataAccessLayer.Context;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("AnimeServiceSqlServer");
builder.Services.AddDbContext<AnimeServiceDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", opt =>
    {
        opt.WithOrigins(builder.Configuration.GetSection("Security:AllowedOrigins").Get<string[]>())
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});
builder.Services.AddMyServices();

builder.Services.AddDataProtection(opt =>
{
    opt.ApplicationDiscriminator = builder.Configuration["Security:CookieProtectKey"];
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
        cfg.UseRawJsonDeserializer();
        cfg.Host(builder.Configuration.GetValue<string>("RabbitMqHost"), "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(cxt);
    });
   
});
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowOrigin");


if (!app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<AnimeServiceDbContext>().Database.Migrate();
}

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
