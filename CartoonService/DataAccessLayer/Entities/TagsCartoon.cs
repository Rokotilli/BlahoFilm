namespace DataAccessLayer.Entities
{
    public class CategoriesCartoon
    {
        public int CartoonId { get; set; }
        public int CategoryId { get; set; }

        public Cartoon Cartoon { get; set; }
        public Category Category { get; set; }
    }
}
