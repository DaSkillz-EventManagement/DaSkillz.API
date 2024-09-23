
namespace Domain.Entities;

public class Question
{
    public Guid QuestionId { get; set; }
    public Guid QuizId { get; set; }
    public string QuestionName { get; set; } = null!;
    public bool IsMultipleAnswers { get; set; } = false;
    public bool IsQuestionAnswered { get; set;} = false;
    public string CorrectAnswerLabel { get; set; } = null!;
    public bool ShowAnswerAfterChoosing { get; set; } = true;

    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ICollection<Answer> Answers { get; set; } = null!;
}
