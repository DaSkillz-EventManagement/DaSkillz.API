namespace Domain.DTOs.Quiz.Response;

public class ResponseAnswerQuizAttemptDto
{
    public Guid AnswerId { get; set; }
    public string? AnswerContent { get; set; } // for non multiplechoice answer
}
