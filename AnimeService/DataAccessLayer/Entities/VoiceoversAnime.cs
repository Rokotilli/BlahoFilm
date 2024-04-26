namespace DataAccessLayer.Entities
{
    public class VoiceoversAnime
    {
        public int AnimeId { get; set; }
        public int VoiceoverId { get; set; }

        public Anime Anime { get; set; }
        public Voiceover Voiceover { get; set; }
    }
}