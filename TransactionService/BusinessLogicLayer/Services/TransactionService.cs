using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace BusinessLogicLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TransactionServiceDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public TransactionService(TransactionServiceDbContext transactionServiceDbContext, IConfiguration configuration, HttpClient httpClient)
        {
            _dbContext = transactionServiceDbContext;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> AddSubscription(SubscriptionModel subscriptionModel, string userId)
        {
            try
            {
                string baseUrl = _configuration["PayPalConfigs:Url"] + "v1/billing/";
                var accessToken = await GetPayPalAccessToken();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{baseUrl}subscriptions/{subscriptionModel.SubscriptionId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<dynamic>(result);

                    var responseplan = await _httpClient.GetAsync($"{baseUrl}plans/{json.plan_id}");

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

                    await _dbContext.Subscriptions.AddAsync(model);
                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                return response.ReasonPhrase;
            }
            catch (Exception ex)
            { 
                return ex.ToString();
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

                string url = _configuration["PayPalConfigs:Url"] + $"v1/billing/subscriptions/{model.Id}/";
                url += model.IsActive ? "suspend" : "activate";                

                var response = await SendHttpRequest(url, reason);

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
                return ex.ToString();
            }
        }

        private async Task<HttpResponseMessage> SendHttpRequest(string url, string reason)
        {
            var accessToken = await GetPayPalAccessToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent("{\"reason\": \"" + reason + "\"}", Encoding.UTF8, "application/json")
            };

            return await _httpClient.SendAsync(httpRequest);
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
