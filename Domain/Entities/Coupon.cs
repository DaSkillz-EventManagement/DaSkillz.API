namespace Domain.Entities
{
    public class Coupon
    {
        public Coupon()
        {
            Events = new HashSet<Event>();
            Users = new HashSet<User>();
        }


        public string Id { get; set; }
        public long CreatedDate { get; set; }
        public long ExpiredDate { get; set; }
        public int NOAttempts { get; set; }
        public string DiscountType { get; set; }
        public decimal Value { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}
