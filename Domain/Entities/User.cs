﻿namespace Domain.Entities
{
    public partial class User
    {
        public User()
        {
            Events = new HashSet<Event>();
            Feedbacks = new HashSet<Feedback>();
            //Notifications = new HashSet<Notification>();
            Participants = new HashSet<Participant>();
            //PaymentTransactions = new HashSet<PaymentTransaction>();
            RefreshTokens = new HashSet<RefreshToken>();
            Coupons = new HashSet<Coupon>();
            AdvertisedEvents = new HashSet<AdvertisedEvent>();
        }

        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        public bool IsPremiumUser { get; set; } = false;


        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        //public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Coupon> Coupons { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<AdvertisedEvent> AdvertisedEvents { get; set; }
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual Subscription? Subscription { get; set; }
    }
}
