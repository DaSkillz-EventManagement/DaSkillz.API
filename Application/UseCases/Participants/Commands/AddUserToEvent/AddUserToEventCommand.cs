using Domain.DTOs.ParticipantDto;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participants.Commands.AddUserToEventCommand
{
    public class AddUserToEventCommand: IRequest<APIResponse>
    {
        public RegisterEventModel RegisterEventModel {  get; set; }
        public Guid UserId { get; set; }
        public AddUserToEventCommand(RegisterEventModel registerEventModel, Guid userId)
        {
            RegisterEventModel = registerEventModel;
            UserId = userId;
        }
    }
}
