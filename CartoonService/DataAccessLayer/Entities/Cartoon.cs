namespace DataAccessLayer.Entities
{
    public class Cartoon
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public byte[] PosterPartOne { get; set; }
        public byte[] PosterPartTwo { get; set; }
        public byte[] PosterPartThree { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CountSeasons { get; set; }
        public int? CountParts { get; set; }
        public string? Duration { get; set; }
        public int AnimationTypeId { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; }
        public string TrailerUri { get; set; }
        public string? FileName { get; set; }
        public string? FileUri { get; set; }
        public int AgeRestriction { get; set; }
        public AnimationType AnimationType { get; set; }
        public ICollection<CartoonPart> CartoonParts { get; set; }
        public ICollection<CategoriesCartoon> CategoriesCartoons { get; set; }
        public ICollection<GenresCartoon> GenresCartoons { get; set; }
        public ICollection<Rating> CartoonRatings { get; set; }
        public ICollection<StudiosCartoon> StudiosCartoons { get; set; }
        public ICollection<SelectionCartoon> SelectionCartoons { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
