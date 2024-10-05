

namespace Domain.Entities;

public class UserAnswer
{
    public Guid UserAnswerId { get; set; }//due to multiple choice question, independence Id is required
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public Guid QuestionId { get; set; }
    public string? AnswerContent { get; set; }
    public string? TotalTime { get; set; }
    public bool? IsCorrect { get; set; }
    public int AttemptNo { get; set; }
    public DateTime? SubmitAt { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;
    public virtual Question Question { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
