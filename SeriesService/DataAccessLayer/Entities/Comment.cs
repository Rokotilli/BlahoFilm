namespace DataAccessLayer.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? SeriesPartId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
        public SeriesPart? SeriesPart { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
        public ICollection<CommentDislike> CommentDislikes { get; set; }
    }
}
