using Domain.DTOs.ParticipantDto;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Participants.Commands.AddUserToEventCommand
{
    public class AddUserToEventCommand : IRequest<APIResponse>
    {
        public RegisterEventModel RegisterEventModel { get; set; }
        public Guid UserId { get; set; }
        public AddUserToEventCommand(RegisterEventModel registerEventModel, Guid userId)
        {
            RegisterEventModel = registerEventModel;
            UserId = userId;
        }
    }
}
