namespace MessageBus.Messages
{
    public class UserReceivedMessage : IntegrationBaseEvent
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
    }
}
