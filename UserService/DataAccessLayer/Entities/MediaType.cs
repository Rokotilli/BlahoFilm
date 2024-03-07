namespace DataAccessLayer.Entities
{
    public class MediaType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MediaWithType> MediaWithTypes { get; set; }
    }
}
