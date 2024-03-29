namespace DataAccessLayer.Entities
{
    public class Subscription
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PlanId { get; set; }
        public string OrderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }

        public User User { get; set; }
    }
}
