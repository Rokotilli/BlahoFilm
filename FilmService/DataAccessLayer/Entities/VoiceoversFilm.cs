namespace DataAccessLayer.Entities
{
    public class VoiceoversFilm
    {
        public int FilmId { get; set; }
        public int VoiceoverId { get; set; }

        public Film Film { get; set; }
        public Voiceover Voiceover { get; set; }
    }
}
