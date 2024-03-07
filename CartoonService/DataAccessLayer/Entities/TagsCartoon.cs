namespace DataAccessLayer.Entities
{
    public class TagsCartoon
    {
        public int CartoonId { get; set; }
        public int TagId { get; set; }

        public Cartoon Cartoon { get; set; }
        public Tag Tag { get; set; }
    }
}
