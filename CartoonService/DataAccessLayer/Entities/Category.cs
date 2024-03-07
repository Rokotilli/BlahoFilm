namespace DataAccessLayer.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Cartoon> Cartoons { get; set; }
    }
}
