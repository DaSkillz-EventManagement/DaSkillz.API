using Domain.DTOs.Quiz.Request;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Quizs.Commands.UpdateQuestion;

public class UpdateQuestionCommand: IRequest<APIResponse>
{
    public List<UpdateQuestionDto> Question { get; set; }
    public Guid QuizId { get; set; }
    public UpdateQuestionCommand(List<UpdateQuestionDto> question, Guid quizId)
    {
        Question = question;
        QuizId = quizId;
    }
}
