namespace DataAccessLayer.Entities
{
    public class Series
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CountSeasons { get; set; }
        public int CountParts { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public double  Rating { get; set; }
        public string Actors { get; set; }
        public string StudioName { get; set; }
        public string TrailerUri { get; set; }

        public ICollection<GenresSeries> GenresSeries { get; set; }
        public ICollection<SeriesPart> SeriesParts { get; set; }
        public ICollection<TagsSeries> TagsSeries { get; set; }
    }
}
