using Domain.DTOs.Events;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Events.Command.CreateEvent
{
    public class CreateEventCommand : IRequest<APIResponse>
    {
        public EventRequestDto EventRequestDto { get; set; }

        public CreateEventCommand(EventRequestDto eventRequestDto)
        {
            EventRequestDto = eventRequestDto;
        }
    }
}
