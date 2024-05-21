using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Entities
{
    public class CategoriesAnime
    {
        public int AnimeId { get; set; }
        public int CategoryId { get; set; }

        public Anime Anime { get; set; }
        public Category Category { get; set; }
    }
}
