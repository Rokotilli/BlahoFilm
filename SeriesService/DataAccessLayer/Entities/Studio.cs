namespace DataAccessLayer.Entities
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<StudiosSeries> StudiosSeries { get; set; }
    }
}