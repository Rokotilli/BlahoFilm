using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class CategoriesFilm : IEntityManyToMany
    {
        public int FilmId { get; set; }
        public int CategoryId { get; set; }

        int IEntityManyToMany.EntityId
        {
            get { return CategoryId; }
            set { CategoryId = value; }
        }

        public Film Film { get; set; }
        public Category Category { get; set; }
    }
}
