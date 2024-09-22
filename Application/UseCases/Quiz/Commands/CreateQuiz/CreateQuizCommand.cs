using Domain.DTOs.Quiz.Request;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Quizs.Commands.CreateQuiz;

public class CreateQuizCommand: IRequest<APIResponse>
{
    public Guid UserId { get; set; }
    public CreateQuizDto QuizDto { get; set; }
    public CreateQuizCommand(Guid userId, CreateQuizDto quizDto)
    {
        UserId = userId;
        QuizDto = quizDto;
    }
}
