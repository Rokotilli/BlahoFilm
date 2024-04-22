namespace DataAccessLayer.Entities
{
    public class StudiosFilm
    {
        public int FilmId { get; set; }
        public int StudioId { get; set; }
        public Film Film { get; set; }
        public Studio Studio { get; set; }
    }
}
