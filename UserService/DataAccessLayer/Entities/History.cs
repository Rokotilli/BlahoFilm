namespace DataAccessLayer.Entities
{
    public class History
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MediaWithTypeId { get; set; }
        public int? PartNumber { get; set; }
        public int? SeasonNumber { get; set; }
        public string TimeCode { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public MediaWithType? MediaWithType { get; set; }
    }
}
