namespace Domain.DTOs.Quiz.Response;

public class ResponseAnswerDto
{
    public Guid AnswerId { get; set; }
    public string? AnswerContent { get; set; } // for non multiplechoice answer
}
