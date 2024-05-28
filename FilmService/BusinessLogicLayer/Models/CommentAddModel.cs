using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class CommentAddModel
    {
        public int FilmId { get; set; }
        public int? ParentCommentId { get; set; }

        [StringLength(500, ErrorMessage = "Max length 500 characters")]
        public string Text { get; set; }
    }
}
