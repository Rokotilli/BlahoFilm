namespace DataAccessLayer.Entities
{
    public class StudiosCartoon
    {
        public int CartoonId { get; set; }
        public int StudioId { get; set; }
        public Cartoon Cartoon{ get; set; }
        public Studio Studio { get; set; }
    }
}