using Domain.Enum.Quiz;
using Domain.Models.Response;
using MediatR;


namespace Application.UseCases.Quizs.Queries.GetQuizByEventId;

public class GetQuizByEventIdQuery : IRequest<APIResponse>
{
    public Guid EventId { get; set; }
    public QuizEnum? Status { get; set; }
    public GetQuizByEventIdQuery(Guid eventId, QuizEnum? status)
    {
        EventId = eventId;
        Status = status;
    }
}
