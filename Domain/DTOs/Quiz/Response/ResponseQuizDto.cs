namespace Domain.DTOs.Quiz.Response;

public class ResponseQuizDto
{
    public Guid QuizId { get; set; }
    public Guid eventId { get; set; }
    public string QuizName { get; set; } = null!;
    public string QuizDescription { get; set; } = null!;
    public Guid CreatedBy { get; set; }
    public DateTime CreateAt { get; set; }
}
