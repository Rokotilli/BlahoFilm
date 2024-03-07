using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Entities
{
    public class GenresAnime
    {
        public int AnimeId { get; set; }
        public int GenreId { get; set; }

        public Anime Anime { get; set; }
        public Genre Genre { get; set; }
    }
}
