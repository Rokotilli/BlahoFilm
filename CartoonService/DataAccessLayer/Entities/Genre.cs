namespace DataAccessLayer.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<GenresCartoon> GenresCartoons { get; set; }
    }
}
