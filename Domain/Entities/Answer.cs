
namespace Domain.Entities;

public class Answer
{
    public Guid AnswerId { get; set; }
    public Guid QuestionId { get; set; }
    public string AnswerLabel { get; set; } = null!; //label for answer. ex: a,b,c,d
    public string Content { get; set; } = null!;
    public bool IsCorrectAnswer { get; set; } = false;
    public virtual Question Question { get; set; } = null!;
}
