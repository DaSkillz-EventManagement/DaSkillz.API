namespace Domain.Entities
{
    public partial class SponsorEvent
    {
        public Guid? EventId { get; set; }
        //public int? SponsorMethodId { get; set; }
        public Guid? UserId { get; set; }
        public string? Status { get; set; }
        public bool? IsSponsored { get; set; }
        public decimal? Amount { get; set; }
        public string? SponsorType { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        //public virtual Event? Event { get; set; } waiting for event to complete
        public virtual User? User { get; set; }
    }
}
