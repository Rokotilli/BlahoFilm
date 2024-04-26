namespace DataAccessLayer.Entities
{
    public class Cartoon
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CountSeasons { get; set; }
        public int? CountParts { get; set; }
        public string? Duration { get; set; }
        public int CategoryId { get; set; }
        public int AnimationTypeId { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; }
        public string TrailerUri { get; set; }
        public string? FileName { get; set; }
        public string? FileUri { get; set; }
        public int AgeRestriction { get; set; }
        public Category Category { get; set; }
        public AnimationType AnimationType { get; set; }
        public ICollection<CartoonPart> Cartoons { get; set; }
        public ICollection<TagsCartoon> TagsCartoons { get; set; }
        public ICollection<GenresCartoon> GenresCartoons { get; set; }
        public ICollection<Rating> CartoonRatings { get; set; }
        public ICollection<StudiosCartoon> StudiosCartoons { get; set; }
        public ICollection<VoiceoversCartoon> VoiceoversCartoons { get; set; }
    }
}
