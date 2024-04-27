namespace DataAccessLayer.Entities
{
    public class Voiceover
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<VoiceoversSeries> Voices { get; set; }
    }
}