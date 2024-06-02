namespace DataAccessLayer.Entities
{
    public class Anime
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
        public DateTime DateOfPublish { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public double Rating { get; set; }
        public string TrailerUri { get; set; }
        public int AgeRestriction { get; set; }
        public string? FileName { get; set; }
        public string? FileUri { get; set; }
        public string? Quality { get; set; }
        public string Country { get; set; }
        public ICollection<AnimePart> AnimeParts { get; set; }
        public ICollection<GenresAnime> GenresAnimes { get; set; }
        public ICollection<CategoriesAnime> CategoriesAnimes { get; set; }
        public ICollection<Rating> AnimeRatings{ get; set; }
        public ICollection<StudiosAnime> StudiosAnime { get; set; }
        public ICollection<SelectionAnime> SelectionAnimes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
