using DataAccessLayer.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Text;
using Newtonsoft.Json;
using MassTransit;
using MessageBus.Messages;
using MessageBus.Outbox.Entities;
using Microsoft.Extensions.Options;
using BusinessLogicLayer.Options;

namespace BusinessLogicLayer.Services
{
    public class SubscriptionBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public SubscriptionBackgroundService(IServiceScopeFactory serviceScopeFactory, IHttpClientFactory httpClientFactory, IOptions<AppSettings> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpClient = httpClientFactory.CreateClient("paypal");
            _appSettings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TransactionServiceDbContext>();
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    var accessToken = await GetPayPalAccessToken();                    

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var model = dbContext.Subscriptions
                    .Where(s => s.EndDate < DateTime.UtcNow && !s.IsExpired);

                    foreach (var item in model)
                    {
                        var response = await _httpClient.GetAsync($"v1/billing/subscriptions/{item.Id}");

                        var result = await response.Content.ReadAsStringAsync();
                        var json = JsonConvert.DeserializeObject<dynamic>(result);

                        var responseplan = await _httpClient.GetAsync($"v1/billing/plans/{item.PlanId}");

                        var resultplan = await responseplan.Content.ReadAsStringAsync();
                        var jsonplan = JsonConvert.DeserializeObject<dynamic>(resultplan);

                        if (json.status == "SUSPENDED" || json.status == "CANCELLED")
                        {
                            item.IsExpired = true;
                            item.IsActive = false;

                            var addedMessage = await dbContext.OutboxMessages.AddAsync(new OutboxMessage(item));

                            try
                            {
                                await publishEndpoint.Publish(new PremiumRemovedMessage { UserId = item.UserId }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);

                                addedMessage.Entity.IsPublished = true;
                            }
                            catch { }

                            await dbContext.SaveChangesAsync();
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
            string clientId = _appSettings.PayPalConfigs.ClientId;
            string secret = _appSettings.PayPalConfigs.Secret;

            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token")
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
