namespace DataAccessLayer.Entities
{
    public class VoiceoversCartoon
    {
        public int CartoonId { get; set; }
        public int VoiceoverId { get; set; }

        public Cartoon Cartoon { get; set; }
        public Voiceover Voiceover { get; set; }
    }
}