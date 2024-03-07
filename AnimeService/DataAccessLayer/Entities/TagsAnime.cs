using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Entities
{
    public class TagsAnime
    {
        public int AnimeId { get; set; }
        public int TagId { get; set; }

        public Anime Anime { get; set; }
        public Tag Tag { get; set; }
    }
}
