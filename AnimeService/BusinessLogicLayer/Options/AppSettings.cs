namespace BusinessLogicLayer.Options
{
    public class ConnectionStrings
    {
        public string AnimeServiceSqlServer { get; set; }
        public string AzureAppConfiguration { get; set; }
    }

    public class Security
    {
        public string JwtIssuer { get; set; }
        public string JwtSecretKey { get; set; }
        public string CookieProtectKey { get; set; }
        public string InitVectorKey { get; set; }
        public string AllowedOrigins { get; set; }
    }

    public class AzureStorage
    {
        public string ConnectionString { get; set; }
        public string AnimeContainerName { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
    }

    public class AppSettings
    {
        public string RabbitMqHost { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public Security Security { get; set; }
        public AzureStorage AzureStorage { get; set; }
    }
}
