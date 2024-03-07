namespace DataAccessLayer.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AnimePartId { get; set; }
        public int? ParentCommentId { get; set; }
        public int CountLikes { get; set; }
        public int CountDislikes { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
        public AnimePart AnimePart { get; set; }
        public Comment ParentComment { get; set; }
    }
}
