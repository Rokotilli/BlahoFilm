namespace DataAccessLayer.Entities
{
    public class Fundraising
    {
        public int Id { get; set; }
        public string FundraisingUrl { get; set; }
        public bool IsClosed { get; set; } = false;
    }
}
