using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Selection : IEntityWithName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public ICollection<SelectionsFilm> SelectionsFilms { get; set; }
    }
}
