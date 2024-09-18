using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Feedbacks.Queries.GetUserFeedBack;

public class GetUserFeedBackQueries: IRequest<APIResponse>
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public GetUserFeedBackQueries(Guid eventId, Guid userId)
    {
        EventId = eventId;
        UserId = userId;
    }
}
