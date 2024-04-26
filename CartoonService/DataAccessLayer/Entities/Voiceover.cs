namespace DataAccessLayer.Entities
{
    public class Voiceover
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<VoiceoversCartoon> Voices { get; set; }
    }
}