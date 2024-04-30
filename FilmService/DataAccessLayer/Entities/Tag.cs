using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Tag : IEntityWithName
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TagsFilm> TagsFilms { get; set; }
    }
}
