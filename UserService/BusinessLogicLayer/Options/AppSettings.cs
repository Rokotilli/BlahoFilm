namespace BusinessLogicLayer.Options
{
    public class Security
    {
        public string JwtIssuer { get; set; }
        public string JwtSecretKey { get; set; }
        public string CookieProtectKey { get; set; }
        public string InitVectorKey { get; set; }
        public string RefreshTokenTTL { get; set; }
        public string AllowedOrigins { get; set; }
    }

    public class Redis
    {
        public string Host { get; set; }
        public string InstanceName { get; set; }
    }

    public class EmailConfirmation
    {
        public string SMTPServerHost { get; set; }
        public string SMTPServerPort { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class ConnectionStrings
    {
        public string UserServiceSqlServer { get; set; }
        public string AzureAppConfiguration { get; set; }
    }

    public class AppSettings
    {
        public string RabbitMqHost { get; set; }
        public Security Security { get; set; }
        public string OAuthGoogleApi { get; set; }
        public Redis Redis { get; set; }
        public EmailConfirmation EmailConfirmation { get; set; }
        public string RedirectUrlToConfirmEmail { get; set; }
        public string RedirectUrlToChangePassword { get; set; }
        public string RedirectUrlToConfirmChangingEmail { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
