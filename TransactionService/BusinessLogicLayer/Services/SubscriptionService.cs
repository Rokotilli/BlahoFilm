using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Options;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MessageBus.Messages;
using MessageBus.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace BusinessLogicLayer.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly TransactionServiceDbContext _dbContext;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public SubscriptionService(TransactionServiceDbContext transactionServiceDbContext, IHttpClientFactory httpClientFactory, IPublishEndpoint publishEndpoint, IOptions<AppSettings> options)
        {
            _dbContext = transactionServiceDbContext;
            _httpClient = httpClientFactory.CreateClient("paypal");
            _publishEndpoint = publishEndpoint;
            _appSettings = options.Value;
        }

        public async Task<string> AddSubscription(SubscriptionModel subscriptionModel, string userId)
        {
            try
            {
                var existSubscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);

                if (existSubscription != null)
                {
                    return "You already have a subscription!";
                }

                var accessToken = await GetPayPalAccessToken();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"v1/billing/subscriptions/{subscriptionModel.SubscriptionId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<dynamic>(result);

                    var responseplan = await _httpClient.GetAsync($"v1/billing/plans/{json.plan_id}");

                    if (!responseplan.IsSuccessStatusCode)
                    {
                        return responseplan.ReasonPhrase;
                    }

                    var resultplan = await responseplan.Content.ReadAsStringAsync();
                    var jsonplan = JsonConvert.DeserializeObject<dynamic>(resultplan);

                    DateTime startDate = DateTime.ParseExact(Convert.ToString(json.start_time), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    var model = new Subscription()
                    {
                        Id = json.id,
                        UserId = userId,
                        PlanId = json.plan_id,
                        OrderId = subscriptionModel.OrderId,
                        StartDate = startDate,
                        EndDate = startDate + TimeSpan.FromDays(Convert.ToDouble(jsonplan.billing_cycles[0].frequency.interval_count)),
                        IsActive = true
                    };

                    var addedSubscription = await _dbContext.Subscriptions.AddAsync(model);

                    var addedMessage = await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(addedSubscription.Entity));

                    try
                    {
                        await _publishEndpoint.Publish(new PremiumReceivedMessage { UserId = model.UserId }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);

                        addedMessage.Entity.IsPublished = true;
                    }
                    catch { }

                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                return response.ReasonPhrase;
            }
            catch
            {
                return "Adding subscription failed!";
            }
        }

        public async Task<string> ChangeStatusSubscription(string userId, string reason)
        {
            try
            {
                var model = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.IsExpired == false);

                if (model == null)
                {
                    return "User doesn't have any subscription";
                }                

                string url = $"v1/billing/subscriptions/{model.Id}/";
                url += model.IsActive ? "suspend" : "activate";

                var accessToken = await GetPayPalAccessToken();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent("{\"reason\": \"" + reason + "\"}", Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                model.IsActive = !model.IsActive;
                _dbContext.Subscriptions.Update(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Changing subscription status failed!";
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
