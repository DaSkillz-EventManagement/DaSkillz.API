
namespace Domain.DTOs.Quiz.Request;

public class UpdateQuizDto
{
    public string QuizName { get; set; } = null!;
    public string QuizDescription { get; set; } = null!;
    public string TotalTime { get; set; } = null!;
    public string status { get; set; } = null!;
    public int AttemptAllow { get; set; } = 1;
}
