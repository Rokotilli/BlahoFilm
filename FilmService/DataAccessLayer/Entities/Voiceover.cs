using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Voiceover : IEntityWithName
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<VoiceoversFilm> VoiceoversFilms { get; set; }
    }
}
