using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class SelectionsFilm : IEntityManyToMany
    {
        public int FilmId { get; set; }
        public int SelectionId { get; set; }

        int IEntityManyToMany.EntityId
        {
            get { return SelectionId; }
            set { SelectionId = value; }
        }

        public Film Film { get; set; }
        public Selection Selection { get; set; }
    }
}
