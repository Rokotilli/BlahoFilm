namespace DataAccessLayer.Entities
{
    public class Rating
    {
        public int FilmId { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }

        public Film Film { get; set; }
        public User User { get; set; }
    }
}
