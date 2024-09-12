using Domain.DTOs.ParticipantDto;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Participants.Commands.UpdateRoleEventCommand
{
    public class UpdateRoleEventCommand: IRequest<APIResponse>
    {
        public RegisterEventModel RegisterEvent { get; set; }
        public Guid ExecutorId { get; set; }
        public UpdateRoleEventCommand(RegisterEventModel registerEvent, Guid executorId)
        {
            RegisterEvent = registerEvent;
            ExecutorId = executorId;
        }
    }
}
