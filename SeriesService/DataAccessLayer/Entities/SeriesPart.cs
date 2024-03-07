namespace DataAccessLayer.Entities
{
    public class SeriesPart
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public int SeasonNumber { get; set; }
        public int PartNumber { get; set; }
        public TimeOnly Duration { get; set; }
        public string FileName { get; set; }
        public string FileUri { get; set; }

        public Series Series { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
