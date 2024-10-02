using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.GetUserAnswers;

public class GetUserAnswersQuery: IRequest<APIResponse>
{
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }

    public GetUserAnswersQuery(Guid userId, Guid quizId)
    {
        UserId = userId;
        QuizId = quizId;
    }
}
