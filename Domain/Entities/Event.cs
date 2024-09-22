using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event
    {
        public Event()
        {
            Participants = new HashSet<Participant>();
            Tags = new HashSet<Tag>();
            Logos = new HashSet<Logo>();
            Coupons = new HashSet<Coupon>();
           
        }

        public Guid Id { get; set; }
        public string EventName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? Image { get; set; }
        public string? Location { get; set; }
        public long? CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public int? Capacity { get; set; }
        public bool Approval { get; set; }
        public decimal? Fare { get; set; }
        public string? LocationUrl { get; set; }
        public string? LocationCoord { get; set; }
        public string? LocationId { get; set; }
        public string? LocationAddress { get; set; }
        public string? Theme { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<AdvertisedEvent?> AdvertisedEvents { get; set; }
        public virtual ICollection<Logo> Logos { get; set; }
        public virtual ICollection<Coupon> Coupons { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Quiz> Quizs { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
