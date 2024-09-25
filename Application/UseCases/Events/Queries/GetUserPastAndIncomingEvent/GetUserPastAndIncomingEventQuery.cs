using Domain.DTOs.Events;
using MediatR;

namespace Application.UseCases.Events.Queries.GetUserPastAndIncomingEvent
{
    public class GetUserPastAndIncomingEventQuery : IRequest<Dictionary<string, List<EventPreviewDto>>>
    {
        public Guid UserId { get; set; }

        public GetUserPastAndIncomingEventQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
