namespace DataAccessLayer.Entities
{
    public class Film
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeOnly Duration { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public int Rating { get; set; }
        public string Actors { get; set; }
        public string StudioName { get; set; }
        public string TrailerUri { get; set; }
        public string FileUri { get; set; }

        public ICollection<GenresFilm> GenresFilms { get; set; }
        public ICollection<TagsFilm> TagsFilms { get; set; }
    }
}
