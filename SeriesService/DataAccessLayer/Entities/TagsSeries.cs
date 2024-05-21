namespace DataAccessLayer.Entities
{
    public class CategoriesSeries
    {
        public int SeriesId { get; set; }
        public int CategoryId { get; set; }

        public Series Series { get; set; }
        public Category Category { get; set; }
    }
}
