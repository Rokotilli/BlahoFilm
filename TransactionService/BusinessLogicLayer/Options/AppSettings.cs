namespace BusinessLogicLayer.Options
{
    public class Security
    {
        public string JwtIssuer { get; set; }
        public string JwtSecretKey { get; set; }
        public string CookieProtectKey { get; set; }
        public string InitVectorKey { get; set; }
        public string AllowedOrigins { get; set; }
    }

    public class ConnectionStrings
    {
        public string TransactionServiceSqlServer { get; set; }
        public string AzureAppConfiguration { get; set; }
    }

    public class PayPalConfigs
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Url { get; set; }
    }

    public class AppSettings
    {
        public string RabbitMqHost { get; set; }
        public Security Security { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public PayPalConfigs PayPalConfigs { get; set; }
    }
}
