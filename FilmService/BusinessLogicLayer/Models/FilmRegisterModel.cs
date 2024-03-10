using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Models
{
    public class FilmRegisterModel
    {
        public IFormFile Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeOnly Duration { get; set; }
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
