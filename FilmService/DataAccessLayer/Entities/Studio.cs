using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Studio : IEntityWithName
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<StudiosFilm> StudiosFilms { get; set; }
    }
}
