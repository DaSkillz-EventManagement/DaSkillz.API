using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.GetAllUsersAnswers;

public class GetAllUsersAnswersQuery: IRequest<APIResponse>
{
    public Guid QuizId { get; set; }

    public GetAllUsersAnswersQuery(Guid quizId)
    {
        QuizId = quizId;
    }
}
