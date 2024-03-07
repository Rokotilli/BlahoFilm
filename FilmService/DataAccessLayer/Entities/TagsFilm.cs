namespace DataAccessLayer.Entities
{
    public class TagsFilm
    {
        public int FilmId { get; set; }
        public int TagId { get; set; }

        public Film Film { get; set; }
        public Tag Tag { get; set; }
    }
}
