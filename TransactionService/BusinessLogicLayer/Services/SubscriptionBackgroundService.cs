using DataAccessLayer.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BusinessLogicLayer.Services
{
    public class SubscriptionBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public SubscriptionBackgroundService(IServiceScopeFactory serviceScopeFactory, HttpClient httpClient, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TransactionServiceDbContext>();
                    string baseUrl = _configuration["PayPalConfigs:Url"] + "v1/billing/";
                    var accessToken = await GetPayPalAccessToken();                    

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var model = dbContext.Subscriptions
                    .Where(s => s.EndDate < DateTime.UtcNow && !s.IsExpired);

                    foreach (var item in model)
                    {
                        var response = await _httpClient.GetAsync($"{baseUrl}subscriptions/{item.Id}");

                        var result = await response.Content.ReadAsStringAsync();
                        var json = JsonConvert.DeserializeObject<dynamic>(result);

                        var responseplan = await _httpClient.GetAsync($"{baseUrl}plans/{item.PlanId}");

                        var resultplan = await responseplan.Content.ReadAsStringAsync();
                        var jsonplan = JsonConvert.DeserializeObject<dynamic>(resultplan);

                        if (json.status == "SUSPENDED" || json.status == "CANCELLED")
                        {
                            item.IsExpired = true;
                            item.IsActive = false;
                        }
                        else
                        {
                            item.EndDate += TimeSpan.FromDays(Convert.ToDouble(jsonplan.billing_cycles[0].frequency.interval_count));
                        }
                    }

                    await dbContext.SaveChangesAsync();

                    await Task.Delay(60000 * 60, stoppingToken);
                }
            }
        }

        private async Task<string> GetPayPalAccessToken()
        {
            string url = _configuration["PayPalConfigs:Url"] + "v1/oauth2/token";
            string clientId = _configuration["PayPalConfigs:ClientId"];
            string secret = _configuration["PayPalConfigs:Secret"];

            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                return JsonNode.Parse(result)["access_token"].ToString();
            }

            return null;
        }
    }
}
