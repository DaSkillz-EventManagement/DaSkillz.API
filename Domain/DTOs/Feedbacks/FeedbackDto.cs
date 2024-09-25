namespace Domain.DTOs.Feedbacks
{
    public class FeedbackDto
    {
        public Guid EventId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
    }
}
