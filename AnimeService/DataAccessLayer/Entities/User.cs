namespace DataAccessLayer.Entities
{
    public class User
    {
        public string UserId { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
