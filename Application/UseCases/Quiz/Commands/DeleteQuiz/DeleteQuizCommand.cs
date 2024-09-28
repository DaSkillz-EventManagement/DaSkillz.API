using Domain.Models.Response;
using MediatR;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Commands.DeleteQuiz;

public class DeleteQuizCommand: IRequest<APIResponse>
{
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }


    public DeleteQuizCommand(Guid quizId, Guid userId)
    {
        QuizId = quizId;
        UserId = userId;
    }
}
