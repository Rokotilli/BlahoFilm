namespace DataAccessLayer.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public int? SubscriptionId { get; set; }
        public int? DonationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
    }
}
