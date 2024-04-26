using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using CartoonServiceAPI.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CartoonServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CartoonServiceSqlServer"));
}); builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", opt =>
    {
        opt.WithOrigins(builder.Configuration.GetSection("Security:AllowedOrigins").Get<string[]>())
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
}); 
builder.Services.AddDataProtection(opt =>
{
    opt.ApplicationDiscriminator = builder.Configuration["Security:CookieProtectKey"];
});
builder.Services.AddControllers();
builder.Services.AddMyServices();
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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<CartoonServiceDbContext>().Database.Migrate();
}

app.MapControllers();
app.Run();
