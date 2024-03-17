namespace DataAccessLayer.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public byte[]? Avatar { get; set; }
        public string TotalTime { get; set; } = "00:00:00";

        public ICollection<History> Histories { get; set; }
        public ICollection<Favorite> Favorites { get; set;}
    }
}
