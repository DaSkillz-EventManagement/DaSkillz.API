

namespace Domain.Entities;

public partial class Feedback
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
    public DateTime? CreatedAt { get; set; }

    public virtual Event Event { get; set; } = null!;
    public virtual User User { get; set; } = null!;

    public Feedback(Guid userId, Guid eventId, string? content, int? rating)
    {
        UserId = userId;
        EventId = eventId;
        Content = content;
        Rating = rating;

    }
}
