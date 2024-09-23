using Domain.DTOs;
using Domain.DTOs.Events.RequestDto;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Events.Command.CreateEvent
{
    public class CreateEventCommand : IRequest<APIResponse>
    {
        public EventRequestDto EventRequestDto { get; set; }
        public Guid UserId {  get; set; }

        public CreateEventCommand(EventRequestDto eventRequestDto, Guid userId)
        {
            EventRequestDto = eventRequestDto;
            UserId = userId;
        }
    }
}
