namespace MessageBus.Messages
{
    public class PremiumRemovedMessage : IntegrationBaseEvent
    {
        public string UserId { get; set; }
    }
}
