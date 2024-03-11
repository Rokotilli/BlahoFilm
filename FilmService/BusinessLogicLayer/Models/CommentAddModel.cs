namespace BusinessLogicLayer.Models
{
    public class CommentAddModel
    {
        public int FilmId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Text { get; set; }
    }
}
