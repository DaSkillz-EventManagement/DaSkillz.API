using Domain.DTOs.User.Response;

namespace Domain.DTOs.Events
{
    public class EventPreviewDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string? Status { get; set; }
        public long StartDate { get; set; }
        public CreatedByUserDto? Host { get; set; } = new CreatedByUserDto();
        public string? Image { get; set; }
        public string? Location { get; set; }
    }
}
