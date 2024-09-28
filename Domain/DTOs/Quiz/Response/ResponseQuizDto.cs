namespace Domain.DTOs.Quiz.Response;

public class ResponseQuizDto
{
    public Guid QuizId { get; set; }
    public Guid eventId { get; set; }
    public string QuizName { get; set; } = null!;
    public string QuizDescription { get; set; } = null!;
    public string TotalTime { get; set; } = null!;
    public string status { get; set; } = null!;
    public int AttemptAllow { get; set; } = 0;
}
