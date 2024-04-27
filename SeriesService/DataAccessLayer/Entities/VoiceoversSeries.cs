namespace DataAccessLayer.Entities
{
    public class VoiceoversSeries
    {
        public int SeriesId { get; set; }
        public int VoiceoverId { get; set; }

        public Series Series { get; set; }
        public Voiceover Voiceover { get; set; }
    }
}