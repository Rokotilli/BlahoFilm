using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Category : IEntityWithName
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CategoriesFilm> CategoriesFilms { get; set; }
    }
}
