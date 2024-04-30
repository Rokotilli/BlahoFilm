using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class TagsFilm : IEntityManyToMany
    {
        public int FilmId { get; set; }
        public int TagId { get; set; }

        int IEntityManyToMany.EntityId
        {
            get { return TagId; }
            set { TagId = value; }
        }

        public Film Film { get; set; }
        public Tag Tag { get; set; }
    }
}
