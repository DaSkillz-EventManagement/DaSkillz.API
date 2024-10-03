using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.ShowQuestion;

public class ShowQuestionQuery: IRequest<APIResponse>
{
    public Guid QuizId { get; set; }

    public ShowQuestionQuery(Guid quizId)
    {
        QuizId = quizId;
    }
}
