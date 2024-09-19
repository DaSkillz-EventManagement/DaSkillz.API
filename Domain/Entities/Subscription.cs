namespace Domain.Entities
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
    }

}
