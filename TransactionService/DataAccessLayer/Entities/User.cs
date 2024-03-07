namespace DataAccessLayer.Entities
{
    public class User
    {
        public string UserId { get; set; }
        
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
