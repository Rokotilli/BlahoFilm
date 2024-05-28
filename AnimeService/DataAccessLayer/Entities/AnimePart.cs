namespace DataAccessLayer.Entities
{
    public class AnimePart
    {
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public int SeasonNumber { get; set; }
        public int PartNumber { get; set; }
        public string? Duration { get; set; }
        public string Quality { get; set; }
        public string? FileName { get; set; }
        public string? FileUri { get; set; }
        public Anime Anime { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
