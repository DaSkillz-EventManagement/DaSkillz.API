using Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Quiz.Response;

public class AllUsersAnswersDto
{
    public AttemptedQuizUserResponse userInfo { get; set; } = new AttemptedQuizUserResponse();
    //public List<UserAnswerResponseDto> userAnswers { get; set; } = new List<UserAnswerResponseDto>();
    //public object userAnswers { get; set; }
    public List<KeyValuePair<string, List<UserAnswerResponseDto>>> userAnswers { get; set; }
}
