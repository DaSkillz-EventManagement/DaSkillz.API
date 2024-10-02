using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Response;

public class UserAnswerResponseDto
{
    public Guid UserAnswerId { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public ResponseQuestionDto Question {  get; set; } = new ResponseQuestionDto();
    public Guid? AnswerId { get; set; }
    public string? AnswerContent { get; set; }
    public string? TotalTime { get; set; }
    public bool? IsCorrect { get; set; }
    public int AttemptNo { get; set; }
}
