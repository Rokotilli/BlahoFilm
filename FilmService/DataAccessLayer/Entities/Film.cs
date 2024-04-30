namespace DataAccessLayer.Entities
{
    public class Film
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public int AgeRestriction { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; } = 0;
        public string Actors { get; set; }
        public string TrailerUri { get; set; }

        public ICollection<GenresFilm> GenresFilms { get; set; }
        public ICollection<TagsFilm> TagsFilms { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<StudiosFilm> StudiosFilms { get; set; }
        public ICollection<VoiceoversFilm> VoiceoversFilms { get; set; }
    }
}
