using Domain.DTOs.Sponsors;
using Domain.DTOs.User.Response;

namespace Domain.DTOs.Events
{
    public class EventDetailDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public List<EventTagDto> eventTags { get; set; } = new List<EventTagDto>();
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public CreatedByUserDto? Host { get; set; } = new CreatedByUserDto();
        public string? Image { get; set; }
        public string? Theme { get; set; }
        public EventLocation? Location { get; set; } = new EventLocation();
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public int? Capacity { get; set; }
        public bool? Approval { get; set; }
        public decimal? Fare { get; set; }
        public List<SponsorLogoDto> sponsorLogos { get; set; } = new List<SponsorLogoDto>();
    }
}
