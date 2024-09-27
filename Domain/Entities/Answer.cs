
namespace Domain.Entities;

public class Answer
{
    public Guid AnswerId { get; set; }
    public Guid QuestionId { get; set; }
    public string? AnswerContent { get; set; } // for non multiplechoice answer
    public bool IsCorrectAnswer { get; set; } = false;
    public virtual Question Question { get; set; } = null!;
}
