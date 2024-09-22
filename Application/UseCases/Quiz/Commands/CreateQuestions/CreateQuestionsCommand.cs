using Domain.DTOs.Quiz.Request;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Commands.CreateQuestions;

public class CreateQuestionsCommand: IRequest<APIResponse>
{
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
    public List<CreateQuestionDto> Dtos { get; set; }
    public CreateQuestionsCommand(Guid quizId, Guid userId, List<CreateQuestionDto> dtos)
    {
        QuizId = quizId;
        UserId = userId;
        Dtos = dtos;
    }

}
