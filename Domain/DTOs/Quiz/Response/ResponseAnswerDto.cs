namespace Domain.DTOs.Quiz.Response;

public class ResponseAnswerDto
{
    public Guid AnswerId { get; set; }
    public Guid QuestionId { get; set; }
    public string AnswerLabel { get; set; } = null!; //label for answer. ex: a,b,c,d
    public string Content { get; set; } = null!;
    public bool IsCorrectAnswer { get; set; } = false;
}
