using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participants.Commands.CheckinParticipant
{
    public class CheckinParticipantCommand: IRequest<APIResponse>
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
