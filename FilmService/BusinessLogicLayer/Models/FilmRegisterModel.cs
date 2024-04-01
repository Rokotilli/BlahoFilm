using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class FilmRegisterModel
    {
        public IFormFile Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"\d+[.](0[0-9]|1[0-9]|2[0-4]):[0-5][0-9]:[0-5][0-9]",
            ErrorMessage = "Duration must be in the format 'd.hh:mm:ss'")]
        public string Duration { get; set; }
        public int AgeRestriction { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public int Rating { get; set; }
        public string Actors { get; set; }
        public string StudioName { get; set; }
        public string TrailerUri { get; set; }
        public string Genres { get; set; }
        public string Tags { get; set; }
    }
}
