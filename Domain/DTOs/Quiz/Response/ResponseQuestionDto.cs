
namespace Domain.DTOs.Quiz.Response;

public class ResponseQuestionDto
{
    public Guid QuestionId { get; set; }
    public Guid QuizId { get; set; }
    public string QuestionName { get; set; } = null!;
    public bool IsMultipleAnswers { get; set; } = false;
    public bool IsQuestionAnswered { get; set; } = false;
    public string CorrectAnswerLabel { get; set; } = null!;
    public bool ShowAnswerAfterChoosing { get; set; } = true;

    public IList<ResponseAnswerDto> Answers { get; set; } = new List<ResponseAnswerDto>();
}
