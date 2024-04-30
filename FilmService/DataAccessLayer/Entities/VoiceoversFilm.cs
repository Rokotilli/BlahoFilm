using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class VoiceoversFilm : IEntityManyToMany
    {
        public int FilmId { get; set; }
        public int VoiceoverId { get; set; }
        public string? FileUri { get; set; }

        int IEntityManyToMany.EntityId
        {
            get { return VoiceoverId; }
            set { VoiceoverId = value; }
        }

        public Film Film { get; set; }
        public Voiceover Voiceover { get; set; }
    }
}
