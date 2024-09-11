using Domain.DTOs.Events;
using MediatR;

namespace Application.UseCases.Events.Command.GetUserPastAndIncomingEvent
{
    public class GetUserPastAndIncomingEventCommand : IRequest<Dictionary<string, List<EventPreviewDto>>>
    {
        public Guid UserId { get; set; }

        public GetUserPastAndIncomingEventCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
