namespace DataAccessLayer.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
