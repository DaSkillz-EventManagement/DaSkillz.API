using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Participants.Commands.CheckinParticipant
{
    public class CheckinParticipantCommand : IRequest<APIResponse>
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public CheckinParticipantCommand(Guid userId, Guid eventId)
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}
