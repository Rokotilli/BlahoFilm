namespace DataAccessLayer.Entities
{
    public class MediaWithType
    {
        public int MediaId { get; set; }
        public int MediaTypeId { get; set; }

        public MediaType MediaType { get; set; }
        public ICollection<History> Histories { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
