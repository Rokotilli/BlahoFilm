namespace DataAccessLayer.Entities
{
    public class TagsSeries
    {
        public int SeriesId { get; set; }
        public int TagId { get; set; }

        public Series Series { get; set; }
        public Tag Tag { get; set; }
    }
}
