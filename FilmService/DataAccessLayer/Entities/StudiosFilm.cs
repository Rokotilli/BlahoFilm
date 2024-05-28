using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class StudiosFilm : IEntityManyToMany
    {
        public int FilmId { get; set; }
        public int StudioId { get; set; }

        int IEntityManyToMany.EntityId
        {
            get { return StudioId; }
            set { StudioId = value; }
        }

        public Film Film { get; set; }
        public Studio Studio { get; set; }
    }
}
