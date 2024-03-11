namespace MessageBus.Messages
{
    public class MediaRegisteredMessage : IntegrationBaseEvent
    {
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
    }
}
