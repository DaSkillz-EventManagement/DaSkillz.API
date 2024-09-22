
using System.ComponentModel.DataAnnotations;


namespace Domain.DTOs.Quiz.Request;

public class CreateQuestionDto
{

    [Required(ErrorMessage = "QuestionName is required!")]
    [MaxLength(500, ErrorMessage = "QuestionName max length is 500 characters!")]
    public string QuestionName { get; set; } = null!;

    [Required(ErrorMessage = "CorrectAnswerLabel is required!")]
    public string CorrectAnswerLabel { get; set; } = null!;
    public bool IsMultipleAnswers { get; set; } = false;

    public bool IsQuestionAnswered { get; set; } = false;

    public bool ShowAnswerAfterChoosing { get; set; } = true;

    [Required(ErrorMessage = "Atleast one answer is required to create a question!")]
    public IList<CreateAnswerDto> Answers { get; set; } = new List<CreateAnswerDto>();
}
