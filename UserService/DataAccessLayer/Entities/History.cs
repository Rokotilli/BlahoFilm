namespace DataAccessLayer.Entities
{
    public class History
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MediaWithTypeId { get; set; }
        public TimeSpan TimeCode { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public MediaWithType? MediaWithType { get; set; }
    }
}
