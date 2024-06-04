namespace DataAccessLayer.Entities
{
    public class Series
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public byte[]? PosterPartOne { get; set; }
        public byte[]? PosterPartTwo { get; set; }
        public byte[]? PosterPartThree { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public int CountSeasons { get; set; }
        public int CountParts { get; set; }
        public DateTime DateOfPublish{ get; set; }
        public string Director { get; set; }
        public double  Rating { get; set; }
        public string Actors { get; set; }
        public string TrailerUri { get; set; }
        public string Country { get; set; }
        public string? Quality { get; set; }
        public int AgeRestriction { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<SeriesPart> SeriesParts { get; set; }
        public ICollection<GenresSeries> GenresSeries { get; set; }
        public ICollection<CategoriesSeries> CategoriesSeries { get; set; }
        public ICollection<StudiosSeries> StudiosSeries { get; set; }
        public ICollection<SelectionSeries> SelectionSeries { get; set; }
    }
}
