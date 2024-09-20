
namespace Domain.Entities;

public class Quiz
{
    public Guid QuizId { get; set; }
    public Guid eventId { get; set; }
    public string QuizName { get; set; } = null!;
    public string QuizDescription { get; set; } = null!;
    public Guid CreatedBy { get; set; }
    public DateTime CreateAt { get; set; }


    public virtual Event Event { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Question> Question { get; set; } = null!;


}
