namespace DataAccessLayer.Entities
{
    public class StudiosAnime
    {
        public int AnimeId { get; set; }
        public int StudioId { get; set; }
        public Anime Anime{ get; set; }
        public Studio Studio { get; set; }
    }
}