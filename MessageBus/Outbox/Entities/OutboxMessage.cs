using System.Text.Json.Serialization;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBus.Outbox.Entities
{
    public class OutboxMessage
    {
        public OutboxMessage(object data) => this.Data = data;
        private OutboxMessage() { }
        public int Id { get; set; }
        private string SerializedData;
        public Type Type { get; set; }
        public bool IsPublished { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public object Data
        {
            get => JsonSerializer.Deserialize(this.SerializedData, this.Type, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            set
            {
                this.Type = value.GetType();
                this.SerializedData = JsonSerializer.Serialize(value, this.Type, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            }
        }
    }
}
