using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Response;

public class ShowQuestionDto
{
    public ResponseQuizDto Quiz { get; set; } = new ResponseQuizDto();
    public List<ResponseQuizAttempt> Questions { get; set; } = new List<ResponseQuizAttempt>();
}
