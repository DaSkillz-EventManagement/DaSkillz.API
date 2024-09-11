using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Events.Command.GetEvent
{
    public class GetEventInfoCommand : IRequest<APIResponse>
    {
        public Guid Id { get; set; }

        public GetEventInfoCommand(Guid id)
        {
            Id = id;
        }
    }
}
