using Domain.DTOs.User.Response;

namespace Domain.DTOs.Feedbacks;

public class FeedbackView
{
    public Guid EventId { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public CreatedByUserDto CreatedBy { get; set; } = new CreatedByUserDto();
}
