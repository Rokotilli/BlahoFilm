namespace DataAccessLayer.Entities
{
    public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MediaWithTypeId { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
        public MediaWithType MediaWithType { get; set; }
    }
}
