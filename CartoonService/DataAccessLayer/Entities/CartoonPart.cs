namespace DataAccessLayer.Entities
{
    public class CartoonPart
    {
        public int Id { get; set; }
        public int CartoonId { get; set; }
        public int? SeasonNumber { get; set; }
        public int? PartNumber { get; set; }
        public string Duration { get; set; }
        public string? FileName { get; set; }
        public string? FileUri { get; set; }
        public string Quality { get; set; }
        public Cartoon Cartoon { get; set; }
    }
}
