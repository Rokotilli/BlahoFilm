namespace BusinessLogicLayer.Models
{
    public class CommentAddModel
    {
        public int? CartoonId { get; set; }
        public int? CartoonPartId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Text { get; set; }
    }
}
