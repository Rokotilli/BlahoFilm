namespace DataAccessLayer.Entities
{
    public class Film
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public byte[]? PosterPartOne { get; set; }
        public byte[]? PosterPartTwo { get; set; }
        public byte[]? PosterPartThree { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Quality { get; set; }
        public string Country { get; set; }
        public int AgeRestriction { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; } = 0;
        public string Actors { get; set; }
        public string? FileUri { get; set; }
        public string? FileName { get; set; }
        public string TrailerUri { get; set; }

        public ICollection<GenresFilm> GenresFilms { get; set; }
        public ICollection<CategoriesFilm> CategoriesFilms { get; set; }
        public ICollection<SelectionsFilm> SelectionsFilms { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<StudiosFilm> StudiosFilms { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
