namespace DataAccessLayer.Entities
{
    public class GenresCartoon
    {
        public int CartoonId { get; set; }
        public int GenreId { get; set; }

        public Cartoon Cartoon { get; set; }
        public Genre Genre { get; set; }
    }
}
