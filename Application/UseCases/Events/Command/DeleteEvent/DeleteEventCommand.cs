using MediatR;

namespace Application.UseCases.Events.Command.DeleteEvent
{
    public class DeleteEventCommand : IRequest<bool>
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public DeleteEventCommand()
        {
        }

        public DeleteEventCommand(Guid eventId, Guid userId)
        {
            EventId = eventId;
            UserId = userId;
        }
    }
}
