using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Quizs.Queries.GetQuizInfo;

public class GetQuizInfoQuery : IRequest<APIResponse>
{
    public Guid QuizId { get; set; }
    public GetQuizInfoQuery(Guid quizId)
    {
        QuizId = quizId;
    }
}
