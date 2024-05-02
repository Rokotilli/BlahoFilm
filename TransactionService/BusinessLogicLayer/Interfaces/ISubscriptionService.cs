using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface ISubscriptionService
    {
        Task<string> AddSubscription(SubscriptionModel subscriptionModel, string userId);
        Task<string> ChangeStatusSubscription(string userId, string reason);
    }
}
