namespace DataAccessLayer.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? CartoonId { get; set; }
        public int? CartoonPartId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Cartoon? Cartoon { get; set; }
        public CartoonPart? CartoonPart { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
        public ICollection<CommentDislike> CommentDislikes { get; set; }
    }
}
