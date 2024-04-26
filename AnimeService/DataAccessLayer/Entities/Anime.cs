namespace DataAccessLayer.Entities
{
    public class Anime
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CountSeasons { get; set; }
        public int? CountParts { get; set; }
        public string? Duration { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; }
        public string TrailerUri { get; set; }
        public int AgeRestriction { get; set; }
        public string FileName { get; set; }
        public string FileUri { get; set; }
        public ICollection<AnimePart> AnimeParts { get; set; }
        public ICollection<GenresAnime> GenresAnimes { get; set; }
        public ICollection<TagsAnime> TagsAnimes { get; set; }
        public ICollection<Rating> AnimeRatings{ get; set; }
        public ICollection<Studio> Studios { get; set; }
        public ICollection<Voiceover> Voiceovers { get; set; }
    }
}
