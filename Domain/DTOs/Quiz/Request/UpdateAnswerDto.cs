using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Request;

public class UpdateAnswerDto
{
    public Guid AnswerId { get; set; }
    public string? AnswerContent { get; set; } // for non multiplechoice answer
    public bool IsCorrectAnswer { get; set; } = false;
}
