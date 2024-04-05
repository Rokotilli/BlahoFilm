namespace BusinessLogicLayer.Models
{
    public class CommentAddModel
    {
        public int? AnimeId { get; set; }
        public int? AnimePartId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Text { get; set; }
    }
}
