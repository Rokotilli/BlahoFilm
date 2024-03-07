namespace DataAccessLayer.Entities
{
    public class Donation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public bool IsClosed { get; set; }
    }
}
