namespace DataAccessLayer.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public string TotalTime { get; set; }

        public ICollection<History> Histories { get; set; }
        public ICollection<Favorite> Favorites { get; set;}
    }
}
