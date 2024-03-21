using MessageBus.Enums;

namespace MessageBus.Messages
{
    public class MediaRegisteredMessage : IntegrationBaseEvent
    {
        public int Id { get; set; }
        public MediaTypes MediaType { get; set; }
    }
}