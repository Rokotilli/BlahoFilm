using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class UserModel
    {
        [EmailAddress(ErrorMessage = "Incorrect syntax, example: \"example@gmail.com\"")]
        [StringLength(40, ErrorMessage = "Max length 50 characters")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z]).+$", ErrorMessage = "Password must contain at least one uppercase letter and a number")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Max length 24 characters, min length 8 characters")]
        public string Password { get; set; }    
    }
}
