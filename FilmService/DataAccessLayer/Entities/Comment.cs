namespace DataAccessLayer.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FilmId { get; set; }
        public string Text { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Film Film { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
        public ICollection<CommentDislike> CommentDislikes { get; set; }
    }
}
