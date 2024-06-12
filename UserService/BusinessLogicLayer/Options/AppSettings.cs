namespace BusinessLogicLayer.Options
{
    public class SecuritySettings
    {
        public string JwtIssuer { get; set; }
        public string JwtSecretKey { get; set; }
        public string CookieProtectKey { get; set; }
        public string InitVectorKey { get; set; }
        public string RefreshTokenTTL { get; set; }
        public string AllowedOrigins { get; set; }
    }

    public class RedisSettings
    {
        public string Host { get; set; }
        public string InstanceName { get; set; }
    }

    public class EmailConfirmationSettings
    {
        public string SMTPServerHost { get; set; }
        public string SMTPServerPort { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class ConnectionStringsSettings
    {
        public string UserServiceSqlServer { get; set; }
        public string AzureAppConfiguration { get; set; }
    }

    public class AppSettings
    {
        public string AllowedHosts { get; set; }
        public string RabbitMqHost { get; set; }
        public SecuritySettings Security { get; set; }
        public string OAuthGoogleApi { get; set; }
        public RedisSettings Redis { get; set; }
        public EmailConfirmationSettings EmailConfirmation { get; set; }
        public string RedirectUrlToConfirmEmail { get; set; }
        public string RedirectUrlToChangePassword { get; set; }
        public string RedirectUrlToConfirmChangingEmail { get; set; }
        public ConnectionStringsSettings ConnectionStrings { get; set; }
    }
}
