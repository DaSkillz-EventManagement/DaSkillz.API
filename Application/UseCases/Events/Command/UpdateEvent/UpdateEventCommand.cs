using Domain.DTOs.Events;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Events.Command.UpdateEvent
{
    public class UpdateEventCommand : IRequest<APIResponse>
    {
        public EventRequestDto EventRequestDto { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }

        public UpdateEventCommand(EventRequestDto eventRequestDto, Guid eventId)
        {
            EventRequestDto = eventRequestDto;
            EventId = eventId;
        }
    }
}
