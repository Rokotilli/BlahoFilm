namespace DataAccessLayer.Entities
{
    public class StudiosSeries
    {
        public int SeriesId { get; set; }
        public int StudioId { get; set; }
        public Series Series{ get; set; }
        public Studio Studio { get; set; }
    }
}