using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Request;

public class AttempQuizDto
{
    public Guid QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
    public string? AnswerContent { get; set; }
}
