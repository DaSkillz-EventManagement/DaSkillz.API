namespace Domain.DTOs.Feedbacks;

public class FeedbackEvent
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
}
