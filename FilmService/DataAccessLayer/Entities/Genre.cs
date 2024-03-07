namespace DataAccessLayer.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<GenresFilm> GenresFilms { get; set; }
    }
}
