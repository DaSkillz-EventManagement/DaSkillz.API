using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Response;

public class ResponseQuizAttempt
{
    public Guid QuestionId { get; set; }
    public Guid QuizId { get; set; }
    public string QuestionName { get; set; } = null!;
    public bool IsMultipleAnswers { get; set; } = false;
    public IList<ResponseAnswerQuizAttemptDto> Answers { get; set; } = new List<ResponseAnswerQuizAttemptDto>();
}
