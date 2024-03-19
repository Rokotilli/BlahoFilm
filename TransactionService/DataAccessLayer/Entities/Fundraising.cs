namespace DataAccessLayer.Entities
{
    public class Fundraising
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public bool IsClosed { get; set; } = false;

        public ICollection<Transaction> Transactions { get; set; }
    }
}
