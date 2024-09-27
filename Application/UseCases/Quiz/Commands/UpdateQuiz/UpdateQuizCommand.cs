using Domain.DTOs.Quiz.Request;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Commands.UpdateQuiz;

public class UpdateQuizCommand : IRequest<APIResponse>
{
    public UpdateQuizDto QuizDto { get; set; }
    public Guid QuizId { get; set; }
    public UpdateQuizCommand(UpdateQuizDto quizDto, Guid quizId)
    {
        QuizDto = quizDto;
        QuizId = quizId;
    }
}
