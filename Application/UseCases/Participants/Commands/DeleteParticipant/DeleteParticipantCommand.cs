using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Participants.Commands.DeleteParticipantCommand
{
    public class DeleteParticipantCommand : IRequest<APIResponse>
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public Guid ExcecutorId { get; set; }
        public DeleteParticipantCommand(Guid userId, Guid eventId, Guid excecutorId)
        {
            UserId = userId;
            EventId = eventId;
            ExcecutorId = excecutorId;
        }
    }
}
