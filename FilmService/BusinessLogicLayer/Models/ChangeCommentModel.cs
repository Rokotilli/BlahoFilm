using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class ChangeCommentModel
    {
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Max length 500 characters")]
        public string Text { get; set; }
    }
}
