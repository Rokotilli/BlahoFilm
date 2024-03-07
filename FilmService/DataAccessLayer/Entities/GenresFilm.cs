namespace DataAccessLayer.Entities
{
    public class GenresFilm
    {
        public int FilmId { get; set; }
        public int GenreId { get; set; }

        public Film Film { get; set; }
        public Genre Genre { get; set; }
    }
}
