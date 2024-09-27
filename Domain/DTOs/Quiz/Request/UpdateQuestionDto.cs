using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Request;

public class UpdateQuestionDto
{
    public Guid QuestionId { get; set; }
    public string QuestionName { get; set; } = null!;
    public bool IsMultipleAnswers { get; set; } = false;
    public string CorrectAnswerLabel { get; set; } = null!;
    public List<UpdateAnswerDto> Answers { get; set; }
}
