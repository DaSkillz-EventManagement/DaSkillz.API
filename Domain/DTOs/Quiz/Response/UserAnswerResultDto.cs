using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Response;

public class UserAnswerResultDto
{
    public Guid QuestionId { get; set; }
    public string QuestionName { get; set; } = null!;
    public bool IsMultipleAnswers { get; set; } = false;
}
