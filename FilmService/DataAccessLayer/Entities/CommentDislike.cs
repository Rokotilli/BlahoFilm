namespace DataAccessLayer.Entities
{
    public class CommentDislike
    {
        public string UserId { get; set; }
        public int CommentId { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}
