using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.GetQuizParticipated;

public class GetQuizParticipatedQuery: IRequest<APIResponse>
{
    public Guid QuizId { get; set; }

    public GetQuizParticipatedQuery(Guid quizId)
    {
        QuizId = quizId;
    }
}
